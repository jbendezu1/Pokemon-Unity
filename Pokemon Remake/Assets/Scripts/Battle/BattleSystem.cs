using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction = 0, currentMove = 0;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        playerHud.SetData(playerUnit.pokemon);
        enemyUnit.Setup();
        enemyHud.SetData(enemyUnit.pokemon);

        dialogBox.SetMoveNames(playerUnit.pokemon.Moves);

        yield return (dialogBox.TypeDialog($"A wild {enemyUnit.pokemon.Base.Name} appeared."));
        yield return new WaitForSeconds(1f);
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("What are you going to do?"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    private void Update()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < 1 || currentAction == 2)
            {
                ++currentAction;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction == 1 || currentAction == 3)
            {
                --currentAction;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1 || currentAction == 1)
            {
                currentAction += 2;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (currentAction == 2 || currentAction == 3)
            {
                currentAction -= 2;
            }
        }
        
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(currentAction == 0)
            {
                PlayerMove();
            }
            if (currentAction == 1)
            {

            }
            if (currentAction == 2)
            {

            }
            if (currentAction == 3)
            {

            }
        }
    } 
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < 1 || currentMove == 2)
            {
                ++currentMove;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove == 1 || currentMove == 3)
            {
                --currentMove;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < 1 || currentMove == 1)
            {
                currentMove += 2;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (currentMove == 2 || currentMove == 3)
            {
                currentMove -= 2;
            }
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentMove == 0)
            {
                
            }
            if (currentMove == 1)
            {

            }
            if (currentMove == 2)
            {

            }
            if (currentMove == 3)
            {

            }
        }
    }
}
