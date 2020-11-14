using System;
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
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> onBattleOver;

    BattleState state;
    int currentAction = 0, currentMove = 0;
    int escapeAttempts = 0;

    PokemonParty playerParty;
    Pokemon wildPokemon;
    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.wildPokemon = wildPokemon;
        this.playerParty = playerParty;
        StartCoroutine(SetupBattle());
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        playerHud.SetData(playerUnit.pokemon);
        enemyUnit.Setup(wildPokemon);
        enemyHud.SetData(enemyUnit.pokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.pokemon.Moves);

        yield return (dialogBox.TypeDialog($"A wild {enemyUnit.pokemon.Base.Name} appeared."));
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("What are you going to do?");
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        var move = playerUnit.pokemon.Moves[currentMove];
        move.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.pokemon.Base.Name} used {move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        enemyUnit.PlayHitAnimation();

        var damageDetails = enemyUnit.pokemon.TakeDamage(move, playerUnit.pokemon);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.pokemon.Base.Name} fainted");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            onBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.pokemon.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"Wild {enemyUnit.pokemon.Base.Name} used {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        playerUnit.PlayHitAnimation();

        var damageDetails = playerUnit.pokemon.TakeDamage(move, enemyUnit.pokemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.pokemon.Base.Name} fainted");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                playerUnit.Setup(nextPokemon);
                playerHud.SetData(nextPokemon);
                
                dialogBox.SetMoveNames(nextPokemon.Moves);

                yield return (dialogBox.TypeDialog($"Go {nextPokemon.Base.Name}."));
                PlayerAction();
            }
            else
                onBattleOver(false);
        }
        else
        {
            dialogBox.EnableDialogText(true);
            dialogBox.EnableMoveSelector(false);
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A critical hit!");

        if (damageDetails.Type > 1f)
            yield return dialogBox.TypeDialog("It's super effective!");
        if (damageDetails.Type < 1f)
            yield return dialogBox.TypeDialog("It's not very effective...");
    }

    public void HandleUpdate()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            dialogBox.EnableDialogText(true);
            dialogBox.EnableMoveSelector(false);
            partyScreen.gameObject.SetActive(false);
            PlayerAction();
        }
        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Return))
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
                OpenPartyScreen();
            }
            if (currentAction == 3)
            {
                StartCoroutine(escape());
            }
        }
    } 
    void HandleMoveSelection()
    {
        new WaitForSeconds(1f);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, playerUnit.pokemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
            
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            dialogBox.EnableDialogText(true);
            dialogBox.EnableMoveSelector(false);
            PlayerAction();
        }
    }
    IEnumerator escape()
    {
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableActionSelector(false);
        if (playerUnit.pokemon.Base.Speed > enemyUnit.pokemon.Base.Speed)
        {
            escapeAttempts = 0;
            yield return dialogBox.TypeDialog("Player succesfully escaped.");
            yield return new WaitForSeconds(2f);
            onBattleOver(true);
        }
        else if ((((playerUnit.pokemon.Base.Speed * 128) / enemyUnit.pokemon.Base.Speed) + 30 * escapeAttempts) % 256 > UnityEngine.Random.Range(0, 256))
        {
            escapeAttempts = 0;
            yield return dialogBox.TypeDialog("Player succesfully escaped.");
            yield return new WaitForSeconds(2f);
            onBattleOver(true);
        }
        else
        {
            escapeAttempts++;
            yield return dialogBox.TypeDialog("Player failed to escape.");
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyMove());
        }
    }
}
