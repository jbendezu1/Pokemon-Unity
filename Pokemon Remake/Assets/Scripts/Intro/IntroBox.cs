using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroBox : MonoBehaviour
{

    [SerializeField] Color highlightedColor;

    [SerializeField] GameObject actionSelector;

    [SerializeField] List<Text> actionTexts;


    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
                actionTexts[i].color = Color.black;
        }
    }

}
