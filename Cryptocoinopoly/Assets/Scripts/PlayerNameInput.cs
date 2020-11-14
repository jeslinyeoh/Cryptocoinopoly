using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    
    public void SavePlayerName()
    {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

    }

    
}
