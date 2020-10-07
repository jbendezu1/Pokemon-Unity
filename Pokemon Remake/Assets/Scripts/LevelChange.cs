using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    /*
        1. Create a trigger and assign this script
        2. Create a trigger where the player is going to be loaded
        3. Complete the script
    */
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string MyLevel = GetComponent<Scene>().name;
        GameObject MyPlayer = GetComponent<GameObject>();
        Transform Destination;
    
    void OnTriggerEnter(Collider other)
    {
            if (other == MyPlayer)
            {
                SceneManager.LoadScene("Region 6");
            }
        }

        void OnLevelWasLoaded()
        {
            MyPlayer.transform.position = Destination.position;
        }
    }

}
