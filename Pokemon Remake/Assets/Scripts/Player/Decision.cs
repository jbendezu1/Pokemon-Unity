using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Decision : MonoBehaviour
{
    public GameObject decision;
    public Button firstButton;
    public GameObject trainer;
    public bool showDecision;


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
            decision.SetActive(true);
            firstButton.Select();
        }
        else
            decision.SetActive(false);
    }

}
