using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Decision : MonoBehaviour
{
    public GameObject decisionBox;
    public Button firstButton;
    public GameObject trainer;
    public string decision;

    // Start is called before the first frame update
    void Start()
    {
        decisionBox.SetActive(false);
    }

    public void ChoseYes()
    {
        decision = "yes";
    }

    public void ChoseNo()
    {
        decision = "no";
    }
}
