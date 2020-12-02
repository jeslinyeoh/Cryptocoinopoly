using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


[RequireComponent(typeof(CharacterController))]
public class TMP: MonoBehaviourPun
{
    [SerializeField] private float movementSpeed;
    private CharacterController controller = null;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.enabled = false;
    }

    private void Update()
    {
     
        if (photonView.IsMine)
        {      
            if (EnableMovment.cnt==0)
            {
                controller.enabled = true;
                PlayerTurnText.playerturntext.text = "YOUR TURN";
                //ShowEndTurn.endturn.SetActive(true);
                TakeInput();
            }
            else
            {
                controller.enabled = false;
                PlayerTurnText.playerturntext.text = "Player"+EnableMovment.cnt+1.ToString()+"turn";
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
