using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Decision : MonoBehaviour
{
    public string decision;

    public void ChoseYes()
    {
        decision = "yes";
    }

    public void ChoseNo()
    {
        decision = "no";
    }
}