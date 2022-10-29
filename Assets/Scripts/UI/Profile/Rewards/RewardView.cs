using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile.Rewards
{
    public class RewardView: UiComponent
    {
        private static readonly string SPRITES_PATH = Configurations.SpritesFolderPath + "SpriteSheet/";
        public string SpriteName;
        public string RewardTitle;
        public string Description;
        public bool Obtained;
        private Image _rewardImage;
        private Transform _notObtainedForeground;

        public void SetData(string spriteName, string title, string description, bool obtained)
        {
            SpriteName = spriteName;
            RewardTitle = title;
            Description = description;
            Obtained = obtained;
        }

        protected override void Initialize()
        {
            _rewardImage = Scripts.Utils.FindChild<Image>(transform, "RewardImage");
            _notObtainedForeground = Scripts.Utils.FindChild<Transform>(transform, "NotObtainedForeground");
        }

        protected override void OnUpdate()
        {
            var foreground = _notObtainedForeground.GetComponent<Image>();
            var color = foreground.color;
            if (Obtained) color.a = 0f;
            else color.a = .75f;
            foreground.color = color;
            
            _rewardImage.sprite = GetRewardSprite();
        }
        
        private Sprite GetRewardSprite()
        {
            string spritePath = SPRITES_PATH + SpriteName;
            return Scripts.Utils.LoadSprite(spritePath);
        }

    }
}