using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DecisionManager : MonoBehaviour
{
    /*
    [SerializeField] GameObject player;
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DecisionManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    bool isTyping;
    Dialog dialog;
    int currentLine = 0;

    public IEnumerator ShowDialog(Dialog dialog)
    {
        Debug.Log("asd");
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));

        List<Pokemon> party = player.GetComponent<PokemonParty>().Pokemons;
        var surfPokemon = party.Find(x => x.Base.Name.Equals("Lapras"));
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }

            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialog(string dialog)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
*/}
