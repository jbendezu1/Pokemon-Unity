using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menuBox;
    [SerializeField] Button firstButton;

    public event Action OnShowMenu;
    public event Action OnCloseMenu;

    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    Menu menu;

    public IEnumerator ShowMenu(Menu menu)
    {
        yield return new WaitForEndOfFrame();

        OnShowMenu?.Invoke();
        this.menu = menu;
        menuBox.SetActive(true);
        firstButton.Select();
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {            
            menuBox.SetActive(false);            
            OnCloseMenu?.Invoke();
        }
    }

    /*
    public IEnumerator SetupButtons(List<string> buttons)
    {
        Vector3 m = transform.position;
        
        for (int i = 0; i < buttons.Count; i++)
        {            
            Vector3 position = new Vector3(menuPanel.position.x, menuPanel.position.y + i, menuPanel.position.z);
            GameObject button = Instantiate(buttonPrefab, position,transform.rotation, menuPanel);
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = buttons[i];
        }
        yield return null;
    }*/
}
