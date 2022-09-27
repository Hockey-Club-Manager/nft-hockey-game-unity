﻿using System.Collections.Generic;
using Near.Models.Tokens;
using Runtime;
using UnityEngine;

namespace UI.Main_menu
{
    public class FirstEntryPopup : MonoBehaviour
    {
        [SerializeField] private Transform cardsContent;
        
        public async void LoadCardsFromPack()
        {
            List<Token> tokens = await Near.MarketplaceContract.ContractMethods.Actions.RegisterAccount();

            foreach (var token in tokens)
            {
                CardInPackUI card = Instantiate(Game.AssetRoot.mainMenuAsset.cardInPackUI, cardsContent);
                card.SetData(token);
            }
        }
    }
}