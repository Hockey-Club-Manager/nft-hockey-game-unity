﻿using System.Collections.Generic;
using Near.Models.Tokens;
using Runtime;
using UI.Main_menu.UIPopups;
using UI.Scripts;
using UnityEngine;

namespace UI.Main_menu
{
    public class FirstEntryPopup : UiComponent
    {
        [SerializeField] private Transform cardsContent;
        private FirstEntryPopupAnimation _firstEntryPopupAnimation;
        
        protected override void Initialize()
        {
            
        }
    }
}