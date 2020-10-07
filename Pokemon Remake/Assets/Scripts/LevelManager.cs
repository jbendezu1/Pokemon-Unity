using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private string level = SceneManager.GetActiveScene().name;
    private static bool created = false;

    // Ensure the script is not deleted while loading.
    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (level == "Region 6")
        {

        }
    }
}
