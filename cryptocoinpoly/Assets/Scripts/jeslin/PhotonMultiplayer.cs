using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PhotonMultiplayer : MonoBehaviourPunCallbacks
{
    [SerializeField] public static int maxroom = 100; //max room =10
    [SerializeField] public static int maxplayer = 2; //max player in a room = 2 (for now)
    [SerializeField] public static int[] roomCheck = new int[100];
    [SerializeField] public static int r; //room number
    [SerializeField] public static int[,] PlayerID = new int[maxroom,maxplayer];
   

    [SerializeField] private TextMeshProUGUI waitingStatusText = null;
    [SerializeField] private int MaxPlayersPerRoom = 0;
    [SerializeField] private int MaxNoOfTurns = 0;

    [SerializeField] private Button startButton = null;

    [SerializeField] private bool createGame = false;
    [SerializeField] private bool joinGame = false;

    [SerializeField] private TextMeshProUGUI[] playerNames = null;

    [SerializeField] private TextMeshProUGUI GameSettingsDisplay = null;

    [SerializeField] private GameObject playerLobby = null;
    [SerializeField] private GameObject popUpMessage = null;
    [SerializeField] private GameObject createJoinOptions = null;

    [SerializeField] private string randomRoomNo = null;
    [SerializeField] private string joinRoomNo = null;
    [SerializeField] private TextMeshProUGUI roomNoDisplay = null;
    [SerializeField] private TMP_InputField roomNoInputField = null;

    //used in OnJoinedRoom()
    ExitGames.Client.Photon.Hashtable turns = new ExitGames.Client.Photon.Hashtable();

    private bool RunFindOpponent = false;

    private bool wasMasterClient = false;

    private const string GameVersion = "0.1";

    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;
        

    public void FindOpponent()
    { 
        Debug.Log("findopponent");
        

        //if u have joined
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Isconnected");


            if (createGame)
            {
                //print host name
                playerNames[0].text = "- " + PhotonNetwork.NickName;

                SetRandomRoomNo();
                roomNoDisplay.text = "Room No: " + randomRoomNo;
                
                PhotonNetwork.CreateRoom(randomRoomNo, new RoomOptions { MaxPlayers = (byte)MaxPlayersPerRoom });
                Debug.Log("Create Game");
                
            }

            else if (joinGame)
            {
                roomNoDisplay.text = "Room No: " + joinRoomNo;
                PhotonNetwork.JoinRoom(joinRoomNo);
                Debug.Log("Join Game");
            }

            
        }

        else
        {
            //run find opponent again
            RunFindOpponent = true;
            Debug.Log("not connected to photon");
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        //to run find opponent again when photon is not connected
        if (RunFindOpponent)
        {
            FindOpponent();
            RunFindOpponent = false;
        }


    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to {cause} ");
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //return to CreateJoinOptions menu
        playerLobby.SetActive(false);
        popUpMessage.SetActive(true);
       
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Player successfully joined the room");

        // assign ID to player (0-2) , r=random room
        for (int i=0; i<maxplayer; i++)     
        {
            PlayerID[r, i] = i;
        }


        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        

        //set max players for non-host
        MaxPlayersPerRoom = PhotonNetwork.CurrentRoom.MaxPlayers;

        if (!wasMasterClient)
        {
            //only player that has created the game can set max no of turns
            if (createGame)
            {
                //set Max No of Turns as Room custom property
                wasMasterClient = true;
                turns.Add("MaxTurnsHash", MaxNoOfTurns);
                PhotonNetwork.CurrentRoom.SetCustomProperties(turns);

            }

            else if (joinGame)
            {
                //other players get MaxNoOfPlayers from room
                MaxNoOfTurns = PhotonNetwork.CurrentRoom.CustomProperties["MaxTurnsHash"].GetHashCode();

            }

        }

        //was master client
        else
        {
            //ex master clients that want to join a new game
            if (joinGame)
            {
                MaxNoOfTurns = PhotonNetwork.CurrentRoom.CustomProperties["MaxTurnsHash"].GetHashCode();
            }
        }


            PrintPlayerNames();
            PrintGameSettings();


            if (playerCount != MaxPlayersPerRoom)
            {
                PrintRemainingPlayerNo();
                Debug.Log("Client is waiting for other players");
            }

            else
            {
                waitingStatusText.text = "All players are in";
            }

         

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PrintRemainingPlayerNo();
        PrintPlayerNames();

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            waitingStatusText.text = "All players are in";

            if (PhotonNetwork.IsMasterClient)
            {
                startButton.interactable = true; //host can start the game
            }

        }

        
    }


    public void PlayerLeaveRoom()
    {
        Debug.Log("Current player leave");
        
        startButton.interactable = false; //host can't start the game once someone left the room

        PhotonNetwork.CurrentRoom.PlayerTtl = 0; //remove player from room if leave room

        PhotonNetwork.LeaveRoom();
        
        if(PhotonNetwork.CurrentRoom.PlayerCount == 0)
        {
            PhotonNetwork.DestroyAll(); //destroy all player objects in the room
            PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0; //remove room if no more players   
        }

        
        //disconnect from main server
        PhotonNetwork.Disconnect();
    }


    //when other players leave the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Other player leave");
        
        //if main client leaves room, clear the name in player room
        if (otherPlayer.IsMasterClient)
        {
            Debug.Log("Master client leave");
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
        }

        
        base.OnPlayerLeftRoom(otherPlayer);

       
        //destroy other player object if current is master client
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyPlayerObjects(otherPlayer);
        }

        PrintRemainingPlayerNo();
        PrintPlayerNames();

        PhotonNetwork.CurrentRoom.IsOpen = true; //reopen the room
        startButton.interactable = false; //host can't start the game
    }


    //load the game scene
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Demoscene"); //rename to the game scene name
    }
    

    public void SetNoPlayers(int x)
    {
        MaxPlayersPerRoom = x + 1;
    }


    public void SetNoTurns(int t)
    {
        MaxNoOfTurns = (t + 1) * 10;
    }


    public void SetCreateGame()
    {
        createGame = true;
        joinGame = false;
    }


    public void SetJoinGame()
    {
        joinGame = true;
        createGame = false;
    }


    public void PrintPlayerNames()
    {
        //clear all previous players names 
        for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < 6; i++)
        {
            playerNames[i].text = null;
        }

        //display all the players name in the current room 
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerNames[i].text = "- " + PhotonNetwork.PlayerList[i].NickName;
        }

        
    }


    public void PrintRemainingPlayerNo()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        waitingStatusText.text = $"Waiting for {MaxPlayersPerRoom - playerCount} more player(s)";
    }


    public void PrintGameSettings()
    {
        GameSettingsDisplay.text = MaxPlayersPerRoom + " players, " + MaxNoOfTurns + " turns";   
    }

    
    public void SetRandomRoomNo()
    {
        
        do
        {
            r = Random.Range(1, 100);
        } while (roomCheck[r-1] != 0);

        if (roomCheck[r - 1] == 0)
        {
            roomCheck[r-1] =1;
        }

        randomRoomNo = r.ToString();
    }


    public void SetJoinRoomNo()
    { 
        joinRoomNo = roomNoInputField.text;    
    }

}
