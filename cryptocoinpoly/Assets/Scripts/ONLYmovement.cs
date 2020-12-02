using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(CharacterController))]
public class ONLYmovement : MonoBehaviourPun
{
    [SerializeField] private float movementSpeed;
    private CharacterController controller = null;
    public int i;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        i=EnableMovment.cnt;
        if (photonView.IsMine && i==1)
        {
            controller.enabled = true;
            PlayerTurnText.playerturntext.text = "Your turn";
            //ShowEndTurn.endturn.SetActive(true);
            TakeInput();
        }
        else
        {
            if (EnableMovment.cnt>1)
            {
                PlayerTurnText.playerturntext.text = "Player" + EnableMovment.cnt+1.ToString() + " turn";
                //ShowEndTurn.endturn.SetActive(false);
            }    
        }
    }


    private void TakeInput()
    {
        var movement = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical"),
        }.normalized;

        controller.SimpleMove(movement * movementSpeed);
    }
}