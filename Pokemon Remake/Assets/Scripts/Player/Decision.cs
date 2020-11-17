using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Decision : MonoBehaviour
{
    public GameObject decisionManager;
    public Button firstButton;
    public GameObject trainer;
    public bool showDecision;
    public string decision;

    Player myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        myPlayer = trainer.GetComponent<Player>();
        showDecision = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (showDecision == true)
        {
            decisionManager.SetActive(true);
            firstButton.Select();
        }
        else
            decisionManager.SetActive(false);
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
