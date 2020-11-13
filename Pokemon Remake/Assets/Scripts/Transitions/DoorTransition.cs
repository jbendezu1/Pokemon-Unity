using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    public Vector2 destination;
    private Animator animator;
    private Animator fade;
    Player myPlayer;

    private void Awake()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fade.SetTrigger("FadeOut");
            myPlayer.hasTeleported = true;
            other.transform.position = destination;
        }
    }
}