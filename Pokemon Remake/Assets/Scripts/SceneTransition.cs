using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    private Animator fade;


    private void Awake()
    {
        fade = GameObject.Find("Fade").GetComponent<Animator>();

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fade.SetTrigger("FadeOut");
            // Store player's last position to get back to it
            playerStorage.initialValue = playerPosition;
            SceneManager.LoadScene(sceneToLoad);

        }
    }
}
