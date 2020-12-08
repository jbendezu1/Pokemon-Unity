using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]

public class Menu : MonoBehaviour
{
    [SerializeField] List<string> buttons;

    public List<string> Buttons
    {
        get { return buttons; }
    }
}
