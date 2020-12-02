using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnText : MonoBehaviour
{
    public static Text playerturntext;

    void Start()
    {
        playerturntext = GetComponent<Text>();
    }
}
