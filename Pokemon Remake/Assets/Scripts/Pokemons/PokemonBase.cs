using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string pokename;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //Base Stats for all pokemon
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int specialAttack;
    [SerializeField] int specialDefense;
    [SerializeField] int speed;



    public string Name
    {
        get { return pokename; }
    }

    public string Description
    {
        get { return description; }
    }
    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }
    public int MaxHP
    {
        get { return maxHP; }
    }

    public int Attack
    {
        get { return attack; }
    }
    public int SpAttack
    {
        get { return specialAttack; }
    }

    public int Defense
    {
        get { return defense; }
    }
    public int SpDefense
    {
        get { return specialDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public PokemonType Type1
    {
        get { return type1; }
    }

    public PokemonType Type2
    {
        get { return type2; }
    }

}



public enum PokemonType
{
    None,
    Normal,
    Grass,
    Fire,
    Water,
    Flying,
    Fighting,
    Electric,
    Rock,
    Ground,
    Steel,
    Ice,
    Poison,
    Psychic,
    Dark,
    Ghost,
    Fairy,
    Dragon,
    Bug
}
