﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene, Menu}

public class GameController : MonoBehaviour
{
    [SerializeField] Player playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] Animator playerAnimation;

    public AudioSource backgroundMusic;

    GameState state;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerController.onEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.OnEnterTrainersView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
            playerAnimation.SetBool("isMoving", false);
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };

        MenuManager.Instance.OnShowMenu += () =>
        {
            state = GameState.Menu;
            playerAnimation.SetBool("isMoving", false);
        };

        MenuManager.Instance.OnCloseMenu += () =>
        {
            if (state == GameState.Menu)
                state = GameState.FreeRoam;
        };
    }

    void EndBattle(bool won)
    {
        

        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        backgroundMusic.Play();
    }

    void StartBattle()
    {
        backgroundMusic.Stop();
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);


        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    TrainerController trainer;
    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        this.trainer = trainer;

        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();


        battleSystem.StartTrainerBattle(playerParty,trainerParty);
    }

    private void Update()
    { 
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            MenuManager.Instance.HandleUpdate();
        }
    }
}
