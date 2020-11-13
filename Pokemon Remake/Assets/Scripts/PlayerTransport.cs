using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransport : MonoBehaviour
{
    public Transform newLocation;
    public GameObject player;
    private Vector3 desiredPosition;
    public Player playerScript;

    void Awake()
    {
        desiredPosition = newLocation.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
    }
 

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position == this.transform.position)
        {
            playerScript.teleporting = true;
            Debug.Log("KOKO");
            Transport();
            playerScript.teleporting = false;
        }
    }

    void Transport()
    {
        player.transform.position = desiredPosition;
        desiredPosition = player.transform.position;
    }
}
