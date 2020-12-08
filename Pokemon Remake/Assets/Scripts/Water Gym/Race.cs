using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Race : MonoBehaviour
{
    public bool finished;
    public Vector2 restartLocation;

    public GameObject trigger;
    public GameObject fadeObject;
    private GameObject player;
    public GameObject timer;

    private Animator playerAnimator;
    private Animator fade;
    private Player myPlayer;
    private Timer raceTimer;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        timer.SetActive(true);
    }

    private void Start()
    {
        finished = false;
        myPlayer = player.GetComponent<Player>();
        raceTimer = timer.GetComponent<Timer>();
    }

    private void Update()
    {
        // The race starts when the trigger is not active
        if (!trigger.activeSelf)
        {
            // Check if timer hasn't reach 0
            if (raceTimer.timerIsRunning)
            {
                // Check if we finished race in time
                if (finished)
                {
                    raceTimer.timerIsRunning = false;
                    StopRace();
                }
            }
            else
            {
                StartCoroutine(MovePlayer(player.transform));
            }
        }
    }

    // Move player to entrance
    IEnumerator MovePlayer(Transform playerTransform)
    {
        fade.SetTrigger("FadeOut");
        playerAnimator.SetBool("isSurfing",false);
        myPlayer.hasTeleported = true;
        myPlayer.canMove = false;
        yield return new WaitForSeconds(0);
        playerTransform.position = restartLocation;
        StopRace();
    }

    public void StopRace()
    {
        trigger.SetActive(true);
        raceTimer.timeRemaining = 180;
        timer.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
