using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject
{
    public Vector2 initialValue;
    public int badgeCount;
    public Inventory playerInventory;
    public PokemonParty myPart;
    public Ordinance playerDirection;
}
