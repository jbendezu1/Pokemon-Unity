using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PokemonBase statBase;
    int level;

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        statBase = pBase;
        level = pLevel;
        HP = statBase.MaxHP;

        Moves = new List<Move>();
        foreach (var move in statBase.LearnableMoves)
        {
            if (move.Level <= level)
                Moves.Add(new Move(move.Base));
            if (Moves.Count >= 4)
                break;
        }
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
    public int MaxHP
    {
        get { return Mathf.FloorToInt((statBase.MaxHP * level) / 100f) + 10; }
    }
    
}
