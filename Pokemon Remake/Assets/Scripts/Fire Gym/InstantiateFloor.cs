using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFloor : MonoBehaviour
{
    public static bool returnPlayer = false;

    private float height = 16;//17
    private float width = 19;//19

    public GameObject floorPrefab;
    private Animator fade;
    GameObject player;
    Player myPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        player = GameObject.Find("Player");

        float xoffest = this.transform.position.x;
        float yoffset = this.transform.position.y;

        for (float y = 0; y < height; y++)
        {
            for (float x = 0; x < width; x++)
            {
                if (x <= 5 && y == 5)
                {
                    continue;
                }

                if (x <= 10 && x >= 7 && y == 9)
                    continue;

                if (x == 1 && y <= 13 && y >= 11)
                    continue;

                if (x <= 18 && x >= 17 && y <= 10 && y >= 8)
                    continue;

                Instantiate(floorPrefab, new Vector3(xoffest + x, yoffset + y,0), Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        myPlayer = player.GetComponent<Player>();
    }


    private void Update()
    {
        if (returnPlayer == true)
        {
            Debug.Log("Going to shadow realm jimbo");
            StartCoroutine(MovePlayer(player.transform));
            returnPlayer = false;
        }
    }

    IEnumerator MovePlayer(Transform playerTransform)
    {
        fade.SetTrigger("FadeOut");
        myPlayer.hasTeleported = true;
        myPlayer.canMove = false;
        yield return new WaitForSeconds(0);
        playerTransform.position = new Vector2(45.5f, 44.8f);

    }
}
