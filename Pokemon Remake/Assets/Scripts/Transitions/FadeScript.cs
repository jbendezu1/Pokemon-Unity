using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public Player myPlayer;

    void EnableMoving()
    {
        myPlayer.canMove = true;
        Debug.Log("Player should move now");
    }
}
