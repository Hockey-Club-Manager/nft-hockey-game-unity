﻿using NearClientUnity.KeyStores;
using NearClientUnity.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using NearClientUnity.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NearClientUnity
{
    public class WalletAccount
    {
        private const string LocalStorageKeySuffix = "_wallet_auth_key";
        private const string LoginWalletUrlSuffix = "/login/";
        private const string PendingAccessKeyPrefix = "pending_key";

        private dynamic _authData = new ExpandoObject();
        private string _authDataKey;
        private IExternalAuthService _authService;
        private IExternalAuthStorage _authStorage;
        private KeyStore _keyStore;        
        private string _networkId;
        private string _walletBaseUrl;

        private Uri _successUri;
        private Uri _failUri;
        private Uri _appUri;

        public WalletAccount(Near near, string appKeyPrefix, IExternalAuthService authService, IExternalAuthStorage authStorage,
            Uri successUri, Uri failUri, Uri appUri)
        {
            _networkId = near.Config.NetworkId;
            _walletBaseUrl = near.Config.WalletUrl;
            appKeyPrefix = string.IsNullOrEmpty(appKeyPrefix) || string.IsNullOrWhiteSpace(appKeyPrefix)
                ? "default"
                : appKeyPrefix;
            _authDataKey = $"{appKeyPrefix}{LocalStorageKeySuffix}";
            _keyStore = (near.Connection.Signer as InMemorySigner).KeyStore;
            _authService = authService;
            _authStorage = authStorage;

            _successUri = successUri;
            _failUri = failUri;
            _appUri = appUri;

            if (_authStorage.HasKey(_authDataKey))
            {
                _authData = JObject.Parse(_authStorage.GetValue(_authDataKey));
            }
            else
            {
                _authData.AccountId = null;
                _authData.AllKeys = "";
            }
        }

        public IExternalAuthStorage NearAuthStorage => _authStorage;

        public async Task CompleteSignIn(string url)
        {
            // TODO: parse URL
            //HttpEncoder.Current = HttpEncoder.Default;
            Uri uri = new Uri(url);
            string[] s = uri.Query.Split('&');
            string publicKey = s[1].Split("=")[1];//HttpUtility.ParseQueryString(uri.Query).Get("public_key");
            string accountId = s[0].Split("=")[1];//HttpUtility.ParseQueryString(uri.Query).Get("account_id");
            string allKeys = s[2].Split("=")[1].Replace("%3A", ":"); //HttpUtility.ParseQueryString(uri.Query).Get("all_keys").Split(',');

            _authData.AccountId = accountId;
            _authData.AllKeys = allKeys;

            try
            {
                _authStorage.Add(_authDataKey, JsonConvert.SerializeObject(_authData));                
                await MoveKeyFromTempToPermanent(accountId, publicKey);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task LoadAccount(string accountId)
        {
            KeyPair keyPair = await _keyStore.GetKeyAsync(_networkId, accountId);
            Debug.Log(keyPair.GetPublicKey());
            Debug.Log(keyPair.ToString());

            _authData.AccountId = accountId;
            _authStorage.Add(_authDataKey, JsonConvert.SerializeObject(_authData));                
        }

        public string GetAccountId()
        {
            return _authData.AccountId ?? "";
        }

        public bool IsSignedIn()
        {
            if (GetAccountId() == "") return false;
            return true;
        }

        public async Task<bool> RequestSignIn(string title)
        {
            if (!string.IsNullOrWhiteSpace(GetAccountId())) return true;
            if (await _keyStore.GetKeyAsync(_networkId, GetAccountId()) != null) return true;

            var accessKey = KeyPair.FromRandom("ed25519");

            var url = new UriBuilder(_walletBaseUrl + LoginWalletUrlSuffix);

            url.Query = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "title", title },
                //{ "contract_id", contractId },
                { "success_url", _successUri.AbsoluteUri },
                { "failure_url", _failUri.AbsoluteUri },
                { "app_url", _appUri.AbsoluteUri},
                { "public_key", accessKey.GetPublicKey().ToString() },
            }).ReadAsStringAsync().Result;

            await _keyStore.SetKeyAsync(_networkId, PendingAccessKeyPrefix + accessKey.GetPublicKey(), accessKey);
            return _authService.OpenUrl(url.Uri.AbsoluteUri);
        }

        public async Task<bool> RegisterAccount(string accountId, AccountCreator accountCreator, string seedPhrase)
        {
            var keysJson = await Web.FetchJsonAsync("https://generator.hockeyclubmanager.com/api/generate-keys", $@"{{""seedphrase"": ""{seedPhrase}"", ""username"": ""{accountId}""}}");
            var privateKey = keysJson["private_key"].ToString();
            KeyPair keyPair = KeyPair.FromString(privateKey);
            await accountCreator.CreateAccountAsync(accountId, keyPair.GetPublicKey());
            
            _authData.AccountId = accountId;
            await _keyStore.SetKeyAsync(_networkId, accountId, keyPair);
            _authStorage.Add(_authDataKey, JsonConvert.SerializeObject(_authData));                
            return true;
        }

        public void SignOut()
        {
            _authData = new ExpandoObject();
            _authData.AccountId = null;
            _authData.AllKeys = null;
            _authStorage.DeleteKey(_authDataKey);
        }

        private async Task MoveKeyFromTempToPermanent(string accountId, string publicKey)
        {
            var pendingAccountId = PendingAccessKeyPrefix + publicKey;
            KeyPair keyPair;
            try
            {
                keyPair = await _keyStore.GetKeyAsync(_networkId, pendingAccountId);
            }
            catch (Exception)
            {
                throw new Exception("Wallet account error: no KeyPair");
            }

            try
            {
                await _keyStore.SetKeyAsync(_networkId, accountId, keyPair);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await _keyStore.RemoveKeyAsync(_networkId, pendingAccountId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<FinalExecutionOutcome> SignAndSendTransactionAsync(string receiverId, Action[] actions, Account account)
        {
            PublicKey localKey = await account.Connection.Signer.GetPublicKeyAsync(account.AccountId, _networkId );
            dynamic accessKey = await AccessKeyForTransaction(account, receiverId, actions, localKey);
            if (accessKey == null)
            {
                throw new Exception($"Cannot find matching key for transaction sent to {receiverId}");
            }

            if (localKey != null && localKey.ToString() == accessKey.public_key.ToString())
            {
                try
                {
                    return await account.SignAndSendTransactionAsync(receiverId, actions);
                }
                catch (Exception e) {
                    // TODO: the exception

                    // var parts = e.Message.Split(':');
                    // if (parts[1] == "NotEnoughAllowance")
                    // {
                    //     accessKey = await this.AccessKeyForTransaction(account, receiverId, actions, null);
                    // }
                    // else {
                    //     throw e;
                    // }
                    try
                    {
                        accessKey = await AccessKeyForTransaction(account, receiverId, actions, null);
                    }
                    catch
                    {
                        throw e;
                    }
                }
            }
            
            // // TODO: block & blockHash
            // // var block = await connection.Provider.GetBlockAsync();
            // ByteArray32 blockHash = default;
            //
            // PublicKey publicKey = new PublicKey(accessKey.public_key);
            // ulong nonce = accessKey.access_key.nonce + 1;
            //
            // Transaction transaction = new Transaction()
            // {
            //     Actions = actions,
            //     BlockHash = blockHash,
            //     Nonce = nonce,
            //     PublicKey = publicKey,
            //     ReceiverId = receiverId,
            //     SignerId = account.AccountId
            // };
            
            var status = await account.Connection.Provider.GetStatusAsync();
            ulong nonce = ulong.Parse(accessKey.access_key.nonce.ToString()) + 1;
            var transaction = await SignedTransaction.TransactionToBase64(receiverId, nonce, actions,
                new ByteArray32() { Buffer = Base58.Decode(status.SyncInfo.LatestBlockHash) }, account.Connection.Signer, account.AccountId, account.Connection.NetworkId);
            
            RequestSignTransaction(transaction);
            return null;
        }
        
        private bool AccessKeyMatchesTransaction(dynamic accessKey, string receiverId, Action[] actions)
        {
            if (accessKey.access_key.permission.ToString() == "FullAccess")
            {
                return true;
            }
            
            // else PermissionType == FunctionCall
            var allowedMethods = accessKey.access_key.permission.FunctionCall.method_names;
            string allowedReceiverId = accessKey.access_key.permission.FunctionCall.receiver_id;
            if (allowedReceiverId == GetAccountId() && allowedMethods.Contains("add_request_and_confirm"))
            {
                return true;
            }

            if (allowedReceiverId == receiverId)
            {
                if (actions.Length != 1)
                {
                    return false;
                }

                var action = actions[0];
                return action != null && (action.Args.Deposit == 0) &&
                       (allowedMethods.Count == 0 || allowedMethods.Contains(action?.Args.MethodName));
            }

            return false;
        }

        private async Task<dynamic> AccessKeyForTransaction(Account account, string receiverId, Action[] actions, PublicKey localKey)
        {
            var rawAccessKeys = await account.GetAccessKeysAsync();
            var accessKeys = new List<dynamic>();

            foreach (dynamic key in rawAccessKeys.keys)
            {
                accessKeys.Add(key);
            }
            
            var accessKey = accessKeys.Find(key => key.public_key != null && key.public_key.ToString() == localKey.ToString());
            
            if (accessKey != null && AccessKeyMatchesTransaction(accessKey, receiverId, actions))
            {
                return accessKey;
            }

            List<string> walletKeys = new List<string>();
            if (_authData.AllKeys is string[])
            {
                walletKeys = _authData.AllKeys.Split(",");
            }
            else
            {
                walletKeys.Add(_authData.AllKeys.ToString());
            }

            foreach (var key in accessKeys)
            {
                if (walletKeys.Contains(key.public_key.ToString()) && AccessKeyMatchesTransaction(key, receiverId, actions))
                {
                    return key;
                }
            }
            
            return null;
        }

        private void RequestSignTransaction(string transaction)
        {
            var url = new UriBuilder(_walletBaseUrl + "/sign");

            url.Query = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "transactions", transaction },
                { "callbackUrl", _appUri.AbsoluteUri}, 
            }).ReadAsStringAsync().Result;

            _authService.OpenUrl(url.Uri.AbsoluteUri);
        }
    }
}