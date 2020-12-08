using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevel : MonoBehaviour
{
    public BattleUnit unit;

    public int Pokelevel()
    {
        if (unit.IsPlayerUnit)
        {
            return 6 + (10*Player.badges);
        }
        else
        {
            return Random.Range(4 + 10 * Player.badges, 8 + 10 * Player.badges);
        }
    }
}
