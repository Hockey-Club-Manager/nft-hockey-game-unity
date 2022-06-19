﻿using UnityEngine;
using UnityEngine.UI;

namespace UI.Marketplace
{
    public abstract class UIPopup : MonoBehaviour
    {
        [SerializeField] private Text title;

        public void Show()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}