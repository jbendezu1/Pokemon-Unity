using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public Button firstButton;
    public GameObject trainer;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toggleMenu();
            if (menu.activeSelf)
                firstButton.Select();
        }
    }

    void toggleMenu()
    {
        if (menu.activeSelf)
            menu.SetActive(false);
        else
            menu.SetActive(true);
        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        DontDestroyOnLoad(trainer);
    }


}
