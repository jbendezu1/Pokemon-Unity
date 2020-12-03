﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;

	[SerializeField] Color highlightedColor;										
    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = "<b>" + pokemon.Base.Name + "</b>";
        levelText.text = "<b>" + pokemon.Level + "</b>";
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);
    }
	public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = highlightedColor;
        else
            nameText.color = Color.black;
    }	 
}
