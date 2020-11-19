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
    Player myPlayer;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        myPlayer = trainer.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            toggleMenu();
            if (menu.activeSelf)
                firstButton.Select();
        }
    }

    void toggleMenu()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            myPlayer.canMove = true;
        }
        else
        {
            menu.SetActive(true);
            myPlayer.canMove = false;
        }

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        DontDestroyOnLoad(trainer);
    }


}
