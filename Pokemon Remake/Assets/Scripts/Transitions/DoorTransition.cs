using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTransition : MonoBehaviour
{
    public float waitTime;
    public Vector2 destination;
    private Animator animator;
    private GameObject fadeObject;
    private Image fadeImage;
    private Animator fade;
    Player myPlayer;

    private void Awake()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        fadeObject = GameObject.Find("Fade");
    }

    private void Start()
    {
        fade = fadeObject.GetComponent<Animator>();
        fadeImage = fadeObject.GetComponent<Image>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MovePlayer(other.transform));
        }
    }

    IEnumerator MovePlayer(Transform playerTransform)
    {
        fade.SetTrigger("FadeOut");
        myPlayer.hasTeleported = true;
        myPlayer.canMove = false;
        yield return new WaitForSeconds(waitTime);
        playerTransform.position = destination;
    }

}