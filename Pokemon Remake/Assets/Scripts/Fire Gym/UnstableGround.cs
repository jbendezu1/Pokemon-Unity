using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnstableGround : MonoBehaviour
{
    private SpriteRenderer currentSprite;
    private Sprite volcanicSprite;
    private Animator fade;

    private float speed = 100f;
    private float amount = 0.01f;
    private float y;
    private float initialY;


    bool willFall = false;
    float fallingTime = 3;
    float recoverTime = 5;
    float setupTime = 10;


    // Start is called before the first frame update
    void Start()
    {
        y = transform.position.y;
        initialY = transform.position.y;
        currentSprite = this.GetComponent<SpriteRenderer>();
        volcanicSprite = currentSprite.sprite;
    }

    void Update()
    {
        if (setupTime <= 0)
        {
            setupTime = 0;
            // If the sprite is fine, randomly see if it will fall
            if (currentSprite.sprite != null && !willFall)
            {
                float rando = Random.Range(0, 1500);

                if (rando == 1)
                {
                    willFall = true;
                }
            }

            // If tile will fall, start shaking!
            if (willFall)
            {
                Shakey();
                fallingTime -= Time.deltaTime;
            }

            // When its time to fall, remove sprite and fix placement to original
            if (fallingTime <= 0)
            {
                willFall = false;
                this.transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
                currentSprite.sprite = null;
                fallingTime = 3;
            }


            // If the sprite is missing start timer for when tile will recover
            if (currentSprite.sprite == null)
            {
                this.gameObject.layer = LayerMask.NameToLayer("Pitfall");
                recoverTime -= Time.deltaTime;
            }
            else
                this.gameObject.layer = LayerMask.NameToLayer("Default");

            // If the timer reaches zero, recover tile and reset timer
            if (recoverTime <= 0)
            {
                currentSprite.sprite = volcanicSprite;
                recoverTime = 5;
            }
        }
        else
            setupTime -= Time.deltaTime;
    }

    // Shake animation
    void Shakey()
    {
        y = this.transform.position.y + Mathf.Sin(Time.time * speed) * amount;
        this.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

}
