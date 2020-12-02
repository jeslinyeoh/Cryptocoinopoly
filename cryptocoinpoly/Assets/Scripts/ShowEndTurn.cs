using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowEndTurn : MonoBehaviour
{
    public static GameObject endturn;

    void Start()
    {
        endturn = GetComponent<GameObject>();
    }

}
