using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;

    public void SetData(Pokemon pokemon)
    {
        nameText.text = "<b>" + pokemon.Base.Name + "</b>";
        levelText.text = "<b>" + pokemon.Level + "</b>";
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
    }
}
