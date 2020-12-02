using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] 
    private GameObject[] playerPrefab = null ;
    [SerializeField] 
    GameObject[] spawnPoints;
    int player = 0;
    

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            player = 1;
        }
        if (player == 1)
        {
            PhotonNetwork.Instantiate(playerPrefab[player].name, spawnPoints[player].transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab[player].name, spawnPoints[player].transform.position, Quaternion.identity);
        }
        
    }

    
   
}
