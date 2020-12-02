using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


[RequireComponent(typeof(CharacterController))]
public class movement : MonoBehaviourPun
{
    [SerializeField] int countdown_cnt;
    [SerializeField] public static float currentTime = 0f;
    [SerializeField] float startingTime = 10f;
    [SerializeField] private float movementSpeed ;
    [SerializeField] public static int maxplayer = 2; //max player in a room = 2 (for now)
    [SerializeField]public CharacterController[] controller = null;

    private void Start()
    {
        int roomNo = PhotonMultiplayer.r;
        currentTime = startingTime;
        countdown_cnt = 0;

        for (int i=0; i<maxplayer;i++)
        {
            controller[i] = GetComponent<CharacterController>();
            controller[i].enabled = false;
        }       
    }

    private void Update()
    {

        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            controller[countdown_cnt].enabled = true;
            if (photonView.IsMine)
            {
                TakeInput(countdown_cnt);
                
            }
             
        }
        else if (currentTime <= 0 && countdown_cnt < 1)
        {
            currentTime = startingTime;
            controller[countdown_cnt].enabled = false;
            countdown_cnt++;
            
        }
    }

    private void TakeInput(int cnt)
    {
        var movement = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical"),
        }.normalized;
       
        controller[cnt].SimpleMove(movement * movementSpeed);
    }


}
