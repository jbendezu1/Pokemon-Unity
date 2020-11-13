using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = "<b>" + pokemon.Base.Name + "</b>";
        levelText.text = "<b>" + pokemon.Level + "</b>";
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
    }
}
