﻿using System;
using System.Threading.Tasks;
using NearClientUnity;
using NearClientUnity.KeyStores;
using UnityEngine;


namespace Near
{
    public class NearPersistentManager : MonoBehaviour
    {
        public static NearPersistentManager Instance { get; private set; }
        public WalletAccount WalletAccount { get; private set; }
        private NearClientUnity.Near _near;
        
        private ContractNear _gameContract;
        private const string GameContactId = "uriyyuriy.testnet";

        private ContractNear _marketplaceContract;
        public readonly string MarketplaceContactId = "new_new_nft_market.testnet";

        private ContractNear _nftContract;
        public readonly string nftContactId = "nft_0_0.testnet";
        
        private async void Awake()
        {
            var dirName = Application.persistentDataPath + "KeyStore";
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _near = new NearClientUnity.Near(config: new NearConfig()
            {
                NetworkId = "testnet",
                NodeUrl = "https://rpc.testnet.near.org",
                ProviderType = ProviderType.JsonRpc,
                SignerType = SignerType.InMemory,
                KeyStore = new UnencryptedFileSystemKeyStore(dirName),
                ContractName = GameContactId,
                WalletUrl = "https://wallet.testnet.near.org"
            });
            WalletAccount = new WalletAccount(
                _near,
                "",
                new AuthService(),
                new AuthStorage(),
                new Uri("nfthockey://testnet.near.org/success"),
                new Uri("nfthockey://testnet.near.org/fail"),
                new Uri("nfthockey://testnet.near.org/"));
        }

        public async Task<ContractNear> GetGameContract()
        {
            if (_gameContract == null)
            {
                _gameContract = await CreateGameContract();
                return _gameContract;
            }   
        
            return _gameContract;
        }
    
        private async Task<ContractNear> CreateGameContract()
        {
            Account account = await Instance.GetAccount();
        
            ContractOptions options = new ContractOptions()
            {
                viewMethods = new[] { "get_available_players", "get_available_games",
                    "is_already_in_the_waiting_list", "get_game_config", "get_game_config"},
                changeMethods = new[] { "make_available", "start_game", "generate_event",
                    "make_unavailable", "internal_stop_game", "get_owner_team", "take_to",
                    "coach_speech", "goalie_out", "goalie_back"}
            };
        
            return new ContractNear(account, WalletAccount, GameContactId, options);
        }
        
        public async Task<ContractNear> GetMarketplaceContract()
        {
            if (_marketplaceContract == null)
            {
                _marketplaceContract = await CreateMarketplaceContract();
                return _marketplaceContract;
            }   
        
            return _marketplaceContract;
        }
    
        private async Task<ContractNear> CreateMarketplaceContract()
        {
            Account account = await Instance.GetAccount();
        
            ContractOptions options = new ContractOptions()
            {
                viewMethods = new[] { "get_sales_by_owner_id", "get_sale",
                    "get_sales_by_nft_contract_id", "storage_paid", "storage_amount"},
                changeMethods = new[] { "update_price", "storage_deposit", "accept_offer", "offer", "remove_sale"}
            };
        
            return new ContractNear(account, WalletAccount, MarketplaceContactId, options);
        }
        
        public async Task<ContractNear> GetNftContract()
        {
            if (_nftContract == null)
            {
                _nftContract = await CreateNftContract();
                return _nftContract;
            }   
        
            return _nftContract;
        }
        
        private async Task<ContractNear> CreateNftContract()
        {
            Account account = await Instance.GetAccount();
        
            ContractOptions options = new ContractOptions()
            {
                viewMethods = new[] { "nft_tokens_for_owner", "nft_tokens_batch",
                    "nft_token", "nft_tokens", "nft_total_supply", "get_owner_nft_team"},
                changeMethods = new[] { "nft_mint", "nft_approve", "insert_nft_field_players", "insert_nft_goalies"}
            };
        
            return new ContractNear(account, WalletAccount, nftContactId, options);
        }

        private async Task<Account> GetAccount()
        {
            return await Instance._near.AccountAsync(WalletAccount.GetAccountId());
        }

        public async Task<AccountState> GetAccountState()
        {
            Account account = await Instance.GetAccount();
            return await account.GetStateAsync();
        }

        public string GetAccountId()
        {
            return WalletAccount.GetAccountId();
        }
        
        public async Task<bool> SignIn()
        {
            return await WalletAccount.RequestSignIn(
                GameContactId,
                "Nft hockey"
            );
        }

        public void SignOut()
        {
            WalletAccount.SignOut();
        }
    }

    public class AuthService : IExternalAuthService
    {
        public bool OpenUrl(string url)
        {
            try
            {
                Application.OpenURL(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class AuthStorage : IExternalAuthStorage
    {  
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Add(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public string GetValue(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}