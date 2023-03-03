using System;
using UI.Profile.Popups;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile.Rewards
{
    public class RewardView: UiComponent, IRewardDataReceiver
    {
        private static readonly string SPRITES_PATH = Configurations.SpritesFolderPath + "SpriteSheets/PlayerCardSpriteSheet/";
        public string SpriteName;
        public string RewardTitle;
        public string Description;
        public bool Obtained;
        private Image _rewardImage;
        private Image _notObtainedForeground; 
        [NonSerialized]public RewardInfoPopup rewardInfoPopup;
        private Button _showPopupButton; 
        private RectTransform profilePlaceParentArea;

        public void SetData(string spriteName, string title, string description, bool obtained)
        {
            SpriteName = spriteName;
            RewardTitle = title;
            Description = description;
            Obtained = obtained;
        }

        protected override void Initialize()
        {
            _rewardImage = Scripts.UiUtils.FindChild<Image>(transform, "RewardImage");
            _notObtainedForeground = Scripts.UiUtils.FindChild<Image>(transform, "NotObtainedForeground");
            _showPopupButton = GetComponent<Button>();
            _showPopupButton.onClick.AddListener(ShowPopup);
            profilePlaceParentArea = UiUtils.FindParent<RectTransform>(transform, "Profile");
        }

        private void ShowPopup()
        {
            if (rewardInfoPopup == null)
            {
                string PATH = Configurations.PrefabsFolderPath + "Popups/Profile/TrophyPopup";
                GameObject prefab = UiUtils.LoadResource<GameObject>(PATH);
                if (rewardInfoPopup != null)
                {
                    Destroy(rewardInfoPopup.gameObject);
                }
                rewardInfoPopup = Instantiate(prefab, profilePlaceParentArea).GetComponent<RewardInfoPopup>();
                rewardInfoPopup.Show(SpriteName, RewardTitle, Description, Obtained);
            }
        }

        protected override void OnUpdate()
        {
            var color = _notObtainedForeground.color;
            if (Obtained) color.a = 0f;
            else color.a = .75f;
            _notObtainedForeground.color = color;
            
            _rewardImage.sprite = GetRewardSprite();
        }
        
        private Sprite GetRewardSprite()
        {
            string spritePath = SPRITES_PATH + SpriteName;
            return Scripts.UiUtils.LoadSprite(spritePath);
        }

    }
}