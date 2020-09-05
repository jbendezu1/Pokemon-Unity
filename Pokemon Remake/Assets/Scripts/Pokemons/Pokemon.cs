using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PokemonBase statBase;
    int level;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        statBase = pBase;
        level = pLevel;
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((statBase.Attack * level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((statBase.Defense * level) / 100f) + 5; }
    }
    public int SpecialAttack
    {
        get { return Mathf.FloorToInt((statBase.SpAttack * level) / 100f) + 5; }
    }
    public int SpecialDefense
    {
        get { return Mathf.FloorToInt((statBase.SpDefense * level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((statBase.Speed * level) / 100f) + 5; }
    }
    public int HP
    {
        get { return Mathf.FloorToInt((statBase.MaxHP * level) / 100f) + 10; }
    }
    
}
