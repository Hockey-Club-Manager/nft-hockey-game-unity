using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawPopup : MonoBehaviour
{
    public GameObject _popup;
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}