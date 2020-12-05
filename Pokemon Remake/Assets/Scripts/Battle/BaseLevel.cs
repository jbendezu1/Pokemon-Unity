using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevel : MonoBehaviour
{
    public BattleUnit unit;
    public Player badges;

    public int Pokelevel()
    {
        if (unit.IsPlayerUnit)
        {
            return 6 + 10*badges.badges;
        }
        else
        {
            return Random.Range(4 + 10 * badges.badges, 8 + 10 * badges.badges);
        }
    }
}
