﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Near;
using NearClientUnity;
using NearClientUnity.Utilities;
using TMPro;
using UI.Main_menu;
using UI.Profile.Models;
using UI.Profile.Popups;
using UI.Profile.Rewards;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Profile
{
    public class Profile : UiComponent
    {
        [Serializable]
        public class UserWallet
        {
            public string name;
            public double balance;
        }
        public UserWallet userWallet;
        
        [Range(1, 5)]
        [SerializeField]
        private int balanceFractionalDisplay = 2;
        
        private TextMeshProUGUI _userWalletName;
        private TextMeshProUGUI _userWalletBalance;
        
        [SerializeField] private TMP_Text LevelNumber;
        [SerializeField] private Slider LevelSlider;
        [SerializeField] private Transform _rewardsParent;
        [SerializeField] private RewardInfoPopup _rewardsInfoPopup;
        [SerializeField] private Transform _createLogoPopup;
        [SerializeField] private SignInView signInView;
        
        private IRewardsRepository _repository = new IndexerRewardsRepository();
        private RewardsUser _rewardsUser;
        private LevelCalculator _levelCalculator;
        private List<BaseReward> _rewardsPrototypes;
        private Button _logoButton;
        private RewardView _rewardViewPrefab;

        //public Button ClosePopup;
        private ILogoLoader _logoLoader = new IndexerLogoLoader();
        private LogoPrefab _logoPrefab;
        private readonly string _pathForm = "/Assets/Sprites/Profile/Form/";
        private readonly string _pathPattern = "/Assets/Sprites/Profile";
        
        private void SetInitialValues()
        {
            LevelNumber.text = _levelCalculator.GetLevelString();
            LevelSlider.value = _levelCalculator.GetLevelProgress();
        }
        
        protected override void Initialize()
        {
            LevelNumber = Scripts.UiUtils.FindChild<TMP_Text>(transform, "LevelNumber");
            LevelSlider = Scripts.UiUtils.FindChild<Slider>(transform, "Progress");
            _rewardsParent = Scripts.UiUtils.FindChild<Transform>(transform, "RewardsContent");
            _rewardsInfoPopup = Scripts.UiUtils.FindChild<RewardInfoPopup>(transform.parent, "TrophyPopup");
            _createLogoPopup = Scripts.UiUtils.FindChild<Transform>(transform.parent, "CreateLogoPopup");
            _logoButton = Scripts.UiUtils.FindChild<Button>(transform, "LogoContainer");
            _levelCalculator = new LevelCalculator(_rewardsUser);
            SetInitialValues();
            _logoButton.onClick.AddListener(() => ShowPopup(_createLogoPopup));
            _userWalletName = Scripts.UiUtils.FindChild<TextMeshProUGUI>(transform, "Wallet");
            _userWalletBalance = Scripts.UiUtils.FindChild<TextMeshProUGUI>(transform, "Balance");
            string path = Configurations.PrefabsFolderPath + "Profile/RewardView";
            _rewardViewPrefab = Scripts.UiUtils.LoadResource<RewardView>(path);
            _logoPrefab = Scripts.UiUtils.FindChild<LogoPrefab>(transform, "Logo");
        }
        
        protected override async void OnAwake()
        {
            _rewardsUser = await _repository.GetUser();
            _rewardsPrototypes = await _repository.GetRewards();
            StartCoroutine(UpdateProfile());
            InitRewards();
        }

        private IEnumerator UpdateProfile()
        {
            while (gameObject.activeSelf)
            {
                OnUpdate();
                yield return new WaitForSeconds(1);
            }
        }
        
        protected override async void OnUpdate()
        {
            userWallet.name = NearPersistentManager.Instance.GetAccountId();
            AccountState accountState = await NearPersistentManager.Instance.GetAccountState();
            userWallet.balance = NearUtils.FormatNearAmount(UInt128.Parse(accountState.Amount));
            _userWalletName.text = userWallet.name;
            string pattern = "{0:0." + new String('0', balanceFractionalDisplay) + "}";
            _userWalletBalance.text = String.Format(pattern, userWallet.balance) + " <sprite name=NearLogo>";
            await Load();
        }
        
        private async Task Load() 
        {
            TeamLogo logoData = await _logoLoader.LoadLogo();
            Debug.Log(logoData.form_name);
            Load(logoData);
        }
        
        private void Load(TeamLogo teamLogo) 
        {
            if (!_logoPrefab.IsDestroyed())
            {
                _logoPrefab.SetData(teamLogo, _pathForm, _pathPattern);
            }
        }
        
        public void GoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ShowPopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(true);
        }
        
        public void ClosePopup(Transform popupTransform)
        {
            popupTransform.gameObject.SetActive(false);
        }
        
        public void SignOut()
        {
            NearPersistentManager.Instance.SignOut();

            gameObject.SetActive(false);
            signInView.gameObject.SetActive(true);
        }

        private void InitRewards()
        {
            foreach (var reward in _rewardsPrototypes)
                CreateReward(reward);
        }

        private RewardView CreateReward(BaseReward reward)
        {
            RewardView rewardView = Instantiate(_rewardViewPrefab, _rewardsParent);
            rewardView.rewardInfoPopup = _rewardsInfoPopup;
            reward.SetForView(rewardView, _rewardsUser);
            return rewardView;
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}