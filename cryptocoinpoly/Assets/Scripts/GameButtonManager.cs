using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;
using System;

public class GameButtonManager : MonoBehaviourPun
{
    
    [SerializeField] Text countdown;
    [SerializeField] public static int PlayerTurn;

    void Start()
    {
        PlayerTurn = 0;
    }

    public void backtomenu()
    {
        PhotonMultiplayer.roomCheck[PhotonMultiplayer.r] = 0;     
        PhotonNetwork.LoadLevel("Menu");
    }

    void Update()
    {
        countdown.text = EnableMovment.currentTime.ToString("0");
    }


}
