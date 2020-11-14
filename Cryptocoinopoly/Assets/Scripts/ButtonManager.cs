using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button nameNextButton = null;

    [SerializeField] private TMP_InputField roomNoInputField = null;
    [SerializeField] private Button RoomNoNextButton = null;

    [SerializeField] private bool noOfPlayersNotNull = false;
    [SerializeField] private bool noOfTurnsNotNull = false;
    [SerializeField] private Button GameSettingsNextButton = null;


    public void EnableButtonInteractable(Button button)
    {
        button.interactable = true;
    }


    public void DisableButtonInteractable(Button button)
    {
        button.interactable = false;
    }


    //check if player enter anything in the input field
    public void CheckNameInputField(string name)
    {
        if (!string.IsNullOrEmpty(name)){
            EnableButtonInteractable(nameNextButton);
        }
    }


    //check if player enter anything in the input field
    public void CheckRoomNoInputField(string no)
    {
        if (!string.IsNullOrEmpty(no)){
            EnableButtonInteractable(RoomNoNextButton);
        }

    }


    //n is not used
    //exists so that we can call this function upon value change of the drop down menu
    //enable Next button when both no of players and turns has been set
    public void CheckGameSettings(int n)
    {
        if (noOfPlayersNotNull && noOfTurnsNotNull)
        {
            EnableButtonInteractable(GameSettingsNextButton);
        }

        else
        {
            DisableButtonInteractable(GameSettingsNextButton);
        }
    }


    public void CheckNoOfPlayers(int p)
    {
        //player selects anything other than the "-" option
        if (p > 0)
        {
            noOfPlayersNotNull = true;
        }

        else
        {
            noOfPlayersNotNull = false;
        }
    }


    public void CheckNoOfTurns(int t)
    {
        //player selects anything other than the "-" option
        if (t > 0)
        {
            noOfTurnsNotNull = true;
        }

        else 
        {
            noOfTurnsNotNull = false;
        }
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
