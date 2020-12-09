using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]

public class Menu : MonoBehaviour
{
    [SerializeField] List<string> buttons;
    [SerializeField] PartyScreen partyScreen;
    PokemonParty playerParty;

    public List<string> Buttons
    {
        get { return buttons; }
    }

    private void Start()
    {
        playerParty = GameObject.Find("Player").GetComponent<PokemonParty>();
    }

    void OpenPartyScreen()
    {
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }
}
