using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableGround : MonoBehaviour
{
    Vector2 startingPos;
    private SpriteRenderer thisSprite;
    private float speed = 100f;
    public float amount = 0.01f;
    private float y;
    bool willFall = false;
    // Start is called before the first frame update
    void Start()
    {
        y = transform.position.y;
        thisSprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // If the sprite is fine, randomly see if it will fall
        if (thisSprite.name != null && !willFall)
        {
            float rando = Random.Range(0, 10);

            if (rando == 1)
            {
                Shakey();
            }
        }

        if (thisSprite.sprite == null)
        {
            thisSprite.sprite = ;
        }
    }



    void Shakey()
    {
        y = startingPos.y + Mathf.Sin(Time.time * speed) * amount;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        willFall = true;
    }
}
