using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PokemonCenter : MonoBehaviour, Interactable
{
    private bool thisIneraction = false;

    [SerializeField] GameObject decisionBox;
    [SerializeField] Button firstButton;
    [SerializeField] Decision decision;
    [SerializeField] GameObject player;
    [SerializeField] Dialog myDialog;

    Animator fade;

    PokemonParty playerParty;

    string goodbye = "Have a good day!";

    void Start()
    {
        fade = GameObject.Find("Fade").GetComponent<Animator>();
    }

    public void Interact(Transform initiator)
    {
        playerParty = player.GetComponent<PokemonParty>();
        string text = "Would you like me to heal your pokemon?";
        thisIneraction = true;
        myDialog.Lines.Clear();
        myDialog.Lines.Insert(0, text);
        StartCoroutine(DialogManager.Instance.ShowDialog(myDialog));
        ShowDecision();
    }

    public void ShowDecision()
    {
        decisionBox.SetActive(true);
        firstButton.Select();
    }

    public void Update()
    {
        if (thisIneraction)
        {
            if (decision.decision == "yes")
            {
                string notification = "Your pokemon are now healed!";

                HealPokemon();

                decision.decision = null;
                thisIneraction = false;
                
                myDialog.Lines.Clear();
                myDialog.Lines.Insert(0, notification);
                StartCoroutine(DialogManager.Instance.ShowDialog(myDialog));

            }

            if (decision.decision == "no")
            {
                myDialog.Lines.Clear();
                myDialog.Lines.Insert(0, goodbye);
                StartCoroutine(DialogManager.Instance.ShowDialog(myDialog));

                decisionBox.SetActive(false);
                decision.decision = null;
                thisIneraction = false;
            }
        }
    }

    public void HealPokemon()
    {
        List<Pokemon> p = playerParty.Pokemons;

        foreach (Pokemon x in p)
        {
            x.HP = x.MaxHp;
        }
        decisionBox.SetActive(false);
    }
}
