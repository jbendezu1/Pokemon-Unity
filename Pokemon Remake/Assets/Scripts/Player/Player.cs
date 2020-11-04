using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    private string spriteName;
    public float moveSpeed;
    private GameObject menu;


    public LayerMask Ocean;
    public LayerMask Entrance;
    public LayerMask Foreground;
    public LayerMask Grass;
    public LayerMask Door;

    private bool isMoving;
    private bool onMilotic;
    private Vector2 input;
    public bool teleporting;
    private Animator animator;
    private SpriteRenderer spriterenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        menu = GameObject.Find("Menu");
        teleporting = false;
    }

    private void Update()
    {
        menu = GameObject.Find("Menu");

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // If player is going left/right lock up/down movement
            if (input.x != 0) input.y = 0;

            // Create player movement
            if (input != Vector2.zero && !menu.activeSelf && !teleporting)
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;
                if (isWalkable(targetPosition))
                    StartCoroutine(Move(targetPosition));
            }
        }
        animator.SetBool("isMoving", isMoving);

    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;

        checkForEncounter();
        checkforDoor();
    }

    // Prevent player from moving over foreground tiles
    private bool isWalkable(Vector3 targetposition) {

        if (Physics2D.OverlapCircle(targetposition, 0.1f, Foreground) != null) {
            return false;
        }

        if (Physics2D.OverlapCircle(targetposition, 0.1f, Ocean) != null && spriterenderer.sprite.name.Contains("Trainer"))
        {
            // Prompt decide to use surf if containing surfable pokemon, otherwise notify player they can't surf
            string spriteName = spriterenderer.sprite.name;
            Debug.Log(spriteName);
            return false;
        }
        return true;
    }

    private void checkforDoor()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, Door) != null)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y+100,transform.position.z);
        }
    }

    private void checkForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, Grass) != null)
        {
            if (Random.Range(1, 101) % 5 == 0)
            {
                Debug.Log("Encountered a wild Pokemon");
            }
        }
    }

}
