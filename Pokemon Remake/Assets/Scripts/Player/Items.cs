using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    public enum ItemType
    {
        Potion,
        Super_Potion,
        Hyper_Potion,
        Full_Potion,
        Paralyze_Heal,
        Antitode,
        Awakening,
        Burn_Heal,
        Pokeball,
        Greatball,
        Ultraball,
        Escape_Rope
    }
    public ItemType itemType;
    public int amount;


}
