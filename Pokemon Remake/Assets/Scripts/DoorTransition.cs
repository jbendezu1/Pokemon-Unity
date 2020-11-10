using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    public Vector2 destination;
    public string movement;
    private Animator animator;
    Player myPlayer;

    private void Awake()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Destination: " + destination + "Position: " + other.transform.position);
            myPlayer.hasTeleported = true;
            //if (movement == "right" && (Input.GetKeyDown(KeyCode.RightArrow) || ))
            // Move player to destination
            other.transform.position = destination;
        }
    }
}
