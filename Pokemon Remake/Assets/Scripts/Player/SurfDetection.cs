using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfDetection : MonoBehaviour
{
    private bool byShore;
    public string incoming;
    public int moveSpeed = 10;
    public Vector3 targetPosition = Vector3.zero;
    private string playerName;
    public Decision playerDecision;
    private Player myPlayer;

    private void Start()
    {
        myPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (byShore)
        {
            var playerPosition = myPlayer.transform.position;
            playerName = myPlayer.GetComponent<SpriteRenderer>().sprite.name;
            targetPosition = new Vector3(Mathf.Ceil(playerPosition.x) + 0.5f, Mathf.Floor(playerPosition.y) + 0.8f, Mathf.Ceil(playerPosition.z) + 0.5f);
            var facingRightWay = AssignTargetPosition(playerName);

            if ( !playerDecision.decisionBox.activeSelf && Input.GetKeyDown(KeyCode.X) && facingRightWay)
            {
                Debug.Log(targetPosition);
                if(targetPosition != Vector3.zero)
                {
                    playerDecision.decisionBox.SetActive(true);
                    playerDecision.firstButton.Select();
                }
            }

            if (playerDecision.decision == "yes")
            {
                Debug.Log("Deided yes and player will be at position: " + targetPosition);
                StartCoroutine(MovePlayer(targetPosition));
                playerDecision.decision = null;
                playerDecision.decisionBox.SetActive(false);
            }
            else if (playerDecision.decision == "no")
            {
                playerDecision.decision = null;
                playerDecision.decisionBox.SetActive(false);
            }
        }
    }

    // Check whether player is coming towards the ocean and sprite is facing towards the ocean
    // If true, then set target position to be in the water

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            byShore = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            byShore = false;
            Debug.Log("We exited");
        }
    }

    bool AssignTargetPosition(string playerName)
    {
        if (incoming == "west")
            if (playerName.Contains("right"))
            {
                targetPosition.x += 1;
                return true;
            }

        if (incoming == "east")
            if (playerName.Contains("left"))
            {
                targetPosition.x -= 1;
                return true;
            }

        if (incoming == "south")
            if (playerName.Contains("back"))
            {
                targetPosition.y += 1;
                return true;
            }

        if (incoming == "north")
            if (playerName.Contains("front"))
            {
                targetPosition.y -= 1;
                return true;
            }
        return false;
    }

    public IEnumerator MovePlayer(Vector3 targetPosition)
    {
        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
