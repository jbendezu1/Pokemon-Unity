using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCState state;
    float idleTime = 0f;
    int currentPattern = 0;
    bool isTrainer = false;
    Character character;
    
    private void Awake()
    {
        character = GetComponent<Character>();
    }
    public void Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            if (character != null)
                character.LookTowards(initiator.position);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                idleTime = 0f;
                state = NPCState.Idle;
            }));
        }
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTime += Time.deltaTime;
            if (idleTime > timeBetweenPattern)
            {
                idleTime = 0;
                if (movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }
            }
        }
        if (character != null)
            character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;
        
        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentPattern],isTrainer);

        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
}

public enum NPCState { Idle, Walking, Dialog }