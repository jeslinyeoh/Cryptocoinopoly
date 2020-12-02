using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(CharacterController))]
public class EnableMovment : MonoBehaviourPun
{
    [SerializeField] public static float currentTime = 0f;
    [SerializeField] float startingTime = 10f;
    [SerializeField] public static int cnt;
    public Text playerturntext;
    public Button endturnbutton;

    void Start()
    {
        currentTime = startingTime;
    }
    private void Update()
    {
        
        if (currentTime>0)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        else if (currentTime<=0)
        {
            currentTime = startingTime;
            cnt++;
        }
    }

}
