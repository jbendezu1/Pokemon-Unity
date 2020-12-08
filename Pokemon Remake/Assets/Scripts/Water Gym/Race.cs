using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Race : MonoBehaviour
{
    public bool finished;
    public Vector2 restartLocation;
    private Animator animator;
    public GameObject fadeObject;
    private GameObject player;

    private Animator fade;
    private Player myPlayer;

    [SerializeField] Timer myTimer;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        animator = player.GetComponent<Animator>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
    }

    private void Start()
    {
        finished = false;
        myPlayer = player.GetComponent<Player>();
        myTimer.timerIsRunning = true;
    }

    private void Update()
    {
        // Check if timer is on
        if (myTimer.timerIsRunning)
        {
            // Check if we finished race
            if (finished)
            {
                myTimer.timerIsRunning = false;
            }
            else
                StartCoroutine(MovePlayer(player.transform));
        }
    }

    // Move player to entrance
    IEnumerator MovePlayer(Transform playerTransform)
    {
        fade.SetTrigger("FadeOut");
        myPlayer.hasTeleported = true;
        myPlayer.canMove = false;
        yield return new WaitForSeconds(0);
        playerTransform.position = restartLocation;
    }
}
