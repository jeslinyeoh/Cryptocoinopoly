using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endturnButton : MonoBehaviour
{
    public void nextplayer()
   {
        int i = EnableMovment.cnt;
        i++;
        EnableMovment.cnt = i;
        EnableMovment.currentTime = 10.0f;
   }

}
