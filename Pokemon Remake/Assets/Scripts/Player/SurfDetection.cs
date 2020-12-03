using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfDetection : MonoBehaviour, Interactable
{
    [SerializeField] GameObject decisionBox;
    [SerializeField] Button firstButton;
    [SerializeField] Decision decision;
    [SerializeField] GameObject player;
    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator)
    {
        Dialog dialog = new Dialog();
        string text = GetSurfablePokemon();
        dialog.Lines.Add(text);
        character.LookTowards(initiator.position);

        foreach (string x in dialog.Lines)
        {
            Debug.Log(x);
        }

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

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
