using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Race : MonoBehaviour
{
    public Vector2 startLocation;
    private Animator animator;
    public GameObject fadeObject;
    private Animator fade;
    Player myPlayer;

    private void Awake()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
        myPlayer = GameObject.Find("Player").GetComponent<Player>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
    }

    // Move player to entrance
    IEnumerator MovePlayer(Transform playerTransform)
    {
        fade.SetTrigger("FadeOut");
        myPlayer.hasTeleported = true;
        myPlayer.canMove = false;
        yield return new WaitForSeconds(0);
        playerTransform.position = startLocation;
    }
}
