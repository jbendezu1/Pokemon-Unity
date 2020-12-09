using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRace : MonoBehaviour
{
    public GameObject race;
    public GameObject timerObject;
    private GameObject player;

    public void Awake()
    {
        player = GameObject.Find("Player");
    }

    public void Update()
    {       
        if (player.transform.position.y >= this.transform.position.y && (player.transform.position.y < (this.transform.position.y + 5)))
            if (player.transform.position.x < this.transform.position.x)
            {
                // If this is the starting line, start the race
                if (this.gameObject.name.Contains("Start"))
                {
                    Debug.Log(" This is the starting line");
                    StartRace();

                }
                // If this is the finish line, set the 
                else if (this.gameObject.name.Contains("Finish"))
                {
                    Debug.Log(" This is the finish line");
                    race.GetComponent<Race>().finished = true;
                }
            }
    }

    public void StartRace()
    {
        race.SetActive(true);
        timerObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}