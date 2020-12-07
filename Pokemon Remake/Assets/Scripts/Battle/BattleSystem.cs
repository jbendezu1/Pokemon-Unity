using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, AboutToUse, BattleOver}
public enum BattleAction { Move, SwitchPokemon, UseItem, Run }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] Image playerImage;
    [SerializeField] Image trainerImage;

    public event Action<bool> OnBattleOver;

    BattleState state;
    BattleState? prevState;
    int currentAction;
    int currentMove;
    int currentMember;
    int escapeAttempts = 0;
    bool aboutToUseChoice = true;

    PokemonParty playerParty;
    PokemonParty trainerParty;
    Pokemon wildPokemon;

    public AudioSource battleMusic;

    bool isTrainerBattle = false;
    Player player;
    TrainerController trainer;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        battleMusic.Play();
        this.wildPokemon = wildPokemon;
        this.playerParty = playerParty;
        StartCoroutine(SetupBattle());
    }

    public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty)
    {
        battleMusic.Play();
        this.trainerParty = trainerParty;
        this.playerParty = playerParty;

        isTrainerBattle = true;
        player = playerParty.GetComponent<Player>();
        trainer = trainerParty.GetComponent<TrainerController>();
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
       //playerUnit.Clear();
        //enemyUnit.Clear();

        if (!isTrainerBattle)
        {
            playerUnit.Setup(playerParty.GetHealthyPokemon());
            enemyUnit.Setup(wildPokemon);
            dialogBox.SetMoveNames(playerUnit.pokemon.Moves);

            yield return (dialogBox.TypeDialog($"A wild {enemyUnit.pokemon.Base.Name} appeared."));
        }

        else
        {
            // trainer battle
            playerUnit.gameObject.SetActive(false);
            enemyUnit.gameObject.SetActive(false);

            playerImage.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(true);
            playerImage.sprite = player.Sprite;
            trainerImage.sprite = trainer.Sprite;

            yield return (dialogBox.TypeDialog($"{trainer.Name} wants to battle."));
            
            trainerImage.gameObject.SetActive(false);
            enemyUnit.gameObject.SetActive(true);
            var enemyPokemon = trainerParty.GetHealthyPokemon();
            enemyUnit.Setup(enemyPokemon);
            yield return (dialogBox.TypeDialog($"{trainer.Name} send out {enemyPokemon.Base.Name}"));

            playerImage.gameObject.SetActive(false);
            playerUnit.gameObject.SetActive(true);
            var playerPokemon = playerParty.GetHealthyPokemon();
            playerUnit.Setup(playerPokemon);
            yield return (dialogBox.TypeDialog($"Go {playerPokemon.Base.Name}"));
            dialogBox.SetMoveNames(playerUnit.pokemon.Moves);

        }

        partyScreen.Init();
        PlayerAction();
    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);
    }

    void PlayerAction()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("What are you going to do?");
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void PlayerMove()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator AboutToUse(Pokemon newPokemon)
    {
        state = BattleState.Busy;
        yield return dialogBox.TypeDialog($"{trainer.Name} is about to use {newPokemon.Base.Name}. Do you want to change pokemon?");

        state = BattleState.AboutToUse;
        dialogBox.EnableChoiceBox(true);
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;
														 
				  
																								   

        if (playerAction == BattleAction.Move)
        {
            playerUnit.pokemon.CurrentMove = playerUnit.pokemon.Moves[currentMove];
            enemyUnit.pokemon.CurrentMove = enemyUnit.pokemon.GetRandomMove();

            int playerMovePriority = playerUnit.pokemon.CurrentMove.Base.Priority;
            int enemyMovePriority = enemyUnit.pokemon.CurrentMove.Base.Priority;
													  

            // Check who goes first
            bool playerGoesFirst = true;
            if (enemyMovePriority > playerMovePriority)
                playerGoesFirst = false;
            else if (enemyMovePriority == playerMovePriority)
                playerGoesFirst = playerUnit.pokemon.Speed >= enemyUnit.pokemon.Speed;

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.pokemon;

            // First Turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.pokemon.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver) yield break;

            if (secondPokemon.HP > 0)
            {
                // Second Turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.pokemon.CurrentMove);
                yield return RunAfterTurn(secondUnit);
                if (state == BattleState.BattleOver) yield break;
            }
        }
        else if (playerAction == BattleAction.SwitchPokemon)
        {
            if (playerAction == BattleAction.SwitchPokemon)
            {
                var selectedPokemon = playerParty.Pokemons[currentMember];
                state = BattleState.Busy;
                yield return SwitchPokemon(selectedPokemon);
            }

            // Enemy Turn
            var enemyMove = enemyUnit.pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }
        else if (playerAction == BattleAction.Run)
        {
            var enemyMove = enemyUnit.pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }

        if (state != BattleState.BattleOver)
            PlayerAction();
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.pokemon.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.pokemon);
            yield return sourceUnit.Hud.UpdateHP();
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.pokemon);

        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.Base.Name} used {move.Base.Name}");

        if (CheckIfMoveHits(move, sourceUnit.pokemon, targetUnit.pokemon))
        {
									  

            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            targetUnit.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffects(move.Base.Effects, sourceUnit.pokemon, targetUnit.pokemon, move.Base.Target);
            }
            else
            {
                var damageDetails = targetUnit.pokemon.TakeDamage(move, sourceUnit.pokemon);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }

            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.pokemon.HP > 0)
															  
									
            {
                foreach (var secondary in move.Base.Secondaries)
                {
                    var rnd = UnityEngine.Random.Range(1, 101);
                    if (rnd <= secondary.Chance)
                        yield return RunMoveEffects(secondary, sourceUnit.pokemon, targetUnit.pokemon, secondary.Target);
                }
            }

            if (targetUnit.pokemon.HP <= 0)
            {
                yield return dialogBox.TypeDialog($"{targetUnit.pokemon.Base.Name} Fainted");
                targetUnit.PlayFaintAnimation();
                yield return new WaitForSeconds(2f);

                CheckForBattleOver(targetUnit);
            }

        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.Base.Name}'s attack missed");
        }
    }

    IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target, MoveTarget moveTarget)
    {
        // Stat Boosting
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
                source.ApplyBoosts(effects.Boosts);
            else
                target.ApplyBoosts(effects.Boosts);
        }

        // Status Condition
        if (effects.Status != ConditionID.none)
        {
            target.SetStatus(effects.Status);
        }

        // Volatile Status Condition
        if (effects.VolatileStatus != ConditionID.none)
        {
            target.SetVolatileStatus(effects.VolatileStatus);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == BattleState.BattleOver) yield break;
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        // Statuses like burn or psn will hurt the pokemon after the turn
        sourceUnit.pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.pokemon);
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.pokemon.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
            yield return new WaitUntil(() => state == BattleState.RunningTurn);
        }
    }

    bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if (move.Base.AlwaysHits)
            return true;

        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

   /* void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
                OpenPartyScreen();
            else
                BattleOver(false);
        }
        else
            BattleOver(true);
    }*/

   void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
                OpenPartyScreen();
            else
                BattleOver(false);
        }
        else
            if (!isTrainerBattle)
        {
            BattleOver(true);
        }
        else
        {
            var nextPokemon = trainerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                StartCoroutine(AboutToUse(nextPokemon));
            }
            else
                BattleOver(true);
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
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }

        else if (state == BattleState.AboutToUse)
        {
            HandleAboutToUse();
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
				prevState = state;				  
                OpenPartyScreen();
            }
            if (currentAction == 3)
            {
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                dialogBox.EnableActionSelector(false);
                StartCoroutine(Run());
                state = BattleState.Busy;
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
			var move = playerUnit.pokemon.Moves[currentMove];
            if (move.PP == 0) return;					 
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(RunTurns(BattleAction.Move));
            
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            dialogBox.EnableDialogText(true);
            dialogBox.EnableMoveSelector(false);
            PlayerAction();
        }
    }
    IEnumerator Run()
    {
        if (isTrainerBattle)
        {
            yield return dialogBox.TypeDialog("You can't run from a trainer battle!");
            state = BattleState.ActionSelection;
        }
        else if (playerUnit.pokemon.Base.Speed > enemyUnit.pokemon.Base.Speed)
        {
            escapeAttempts = 0;
            yield return dialogBox.TypeDialog("Player succesfully escaped.");
            yield return new WaitForSeconds(2f);
            BattleOver(true);
        }
        else if ((((playerUnit.pokemon.Base.Speed * 128) / enemyUnit.pokemon.Base.Speed) + 30 * escapeAttempts) % 256 > UnityEngine.Random.Range(0, 256))
        {
            escapeAttempts = 0;
            yield return dialogBox.TypeDialog("Player succesfully escaped.");
            yield return new WaitForSeconds(2f);
            BattleOver(true);
        }
        else
        {
            escapeAttempts++;
            yield return dialogBox.TypeDialog("Player failed to escape.");
            yield return new WaitForSeconds(2f);
            StartCoroutine(RunTurns(BattleAction.Run));
        }
    }
	    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            var selectedMember = playerParty.Pokemons[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon");
                return;
            }
            if (selectedMember == playerUnit.pokemon)
            {
                partyScreen.SetMessageText("You can't switch with the same pokemon");
                return;
            }

            partyScreen.gameObject.SetActive(false);

            if (prevState == BattleState.ActionSelection)
            {
                prevState = null;
                StartCoroutine(RunTurns(BattleAction.SwitchPokemon));
            }
            else
            {
                state = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedMember));
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (playerUnit.pokemon.HP <= 0)
            {
                partyScreen.SetMessageText("You must choose a pokemon to continue");
                return;
            }
            partyScreen.gameObject.SetActive(false);
            if (prevState == BattleState.AboutToUse)
            {
                prevState = null;
                StartCoroutine(SendNextTrainerPokemon());
            }
            else
                PlayerAction();					   
        }
    }

    void HandleAboutToUse()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            aboutToUseChoice = !aboutToUseChoice;

        dialogBox.UpdateChoiceBox(aboutToUseChoice);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableChoiceBox(false);
            if (aboutToUseChoice == true)
            {
                // Yes Option
                prevState = BattleState.AboutToUse;
                OpenPartyScreen();
            }
            else
            {
                // No Option
                StartCoroutine(SendNextTrainerPokemon());
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightShift))
        {
            dialogBox.EnableChoiceBox(false);
            StartCoroutine(SendNextTrainerPokemon());
        }
    }

    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        if (playerUnit.pokemon.HP > 0)
        {
							 
            yield return dialogBox.TypeDialog($"Come back {playerUnit.pokemon.Base.Name}");
            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
										
        }

        playerUnit.Setup(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);
        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}!");
        if (prevState == null)
            state = BattleState.RunningTurn;
        else if (prevState == BattleState.AboutToUse)
        {
            StartCoroutine(SendNextTrainerPokemon());
            prevState = null;
        }
    }

    IEnumerator SendNextTrainerPokemon()
    {
        state = BattleState.Busy;
        var nextPokemon = trainerParty.GetHealthyPokemon();
        enemyUnit.Setup(nextPokemon);
        yield return dialogBox.TypeDialog($"{trainer.Name} send out {nextPokemon.Base.Name}!");

        state = BattleState.RunningTurn;

    }
}
