using System.Collections.Generic;
using Runtime;
using UnityEngine;

namespace UI.Marketplace
{
    public class MarketplaceInteractor : ViewInteractor
    {
        [SerializeField] private List<Transform> views;

        public override void ChangeView(Transform toView)
        {
            foreach (Transform view in views)
            {
                view.gameObject.SetActive(false);
            }
            
            toView.gameObject.SetActive(true);
        }

        public void Back()
        {
            Game.LoadMarketplace();
        }
    }
}