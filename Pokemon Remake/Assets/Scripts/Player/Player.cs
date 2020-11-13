using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{
    public bool hasTeleported;
    private string spriteName;
    public float moveSpeed;
    private Rigidbody2D myRigidBody;

    private Image fade;
    private Animator fadeAnimator;
    private GameObject menu;
    public VectorValue startingPosition;

    public int badges = 0;

    public event Action onEncountered;

    public LayerMask Ocean;
    public LayerMask Foreground;
    public LayerMask Grass;
    public LayerMask Door;
    public LayerMask interactableLayer;

    private bool isMoving;
    private bool onMilotic;
    private Vector3 input;

    private Animator animator;
    private SpriteRenderer spriterenderer;
    public Coroutine co;

    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    private void Awake()
    {
        Debug.Log("Awake");
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        fade = GameObject.Find("Fade").GetComponent<Image>();
        fadeAnimator = GameObject.Find("Fade").GetComponent<Animator>();
        menu = GameObject.Find("Menu");
 //       inventory = new Inventory();
 //       uiInventory.SetInventory(inventory);
    }

    private void Start()
    {
        transform.position = startingPosition.initialValue;
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // If player is going left/right lock up/down movement
            if (input.x != 0) input.y = 0;

            // Create player movement
            if (input != Vector3.zero && !menu.activeSelf && !fade.IsActive())
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                if (isWalkable(targetPosition))
                {
                    co = StartCoroutine(MovePlayer(targetPosition));                    
                }
            }
        }
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();

        if (fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fade Out"))
            fadeAnimator.SetTrigger("FadeIn");
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    public IEnumerator MovePlayer(Vector3 targetPosition)
    { 
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            if (hasTeleported)
                break;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (!hasTeleported)
        {
            transform.position = targetPosition;
        }

        hasTeleported = false;
        isMoving = false;
        checkForEncounter();            
        
    }

    // Prevent player from moving over foreground tiles
    private bool isWalkable(Vector3 targetposition) {

        if (Physics2D.OverlapCircle(targetposition, 0.1f, Foreground | interactableLayer) != null) {
            return false;
        }

        if (Physics2D.OverlapCircle(targetposition, 0.1f, Ocean) != null && spriterenderer.sprite.name.Contains("Trainer"))
        {
            // Prompt decide to use surf if containing surfable pokemon, otherwise notify player they can't surf
            string spriteName = spriterenderer.sprite.name;
            return false;
        }
        return true;
    }

    private void checkForEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, Grass) != null)
        {
            if (UnityEngine.Random.Range(1, 101) % 10 == 0)
            {
                animator.SetBool("isMoving", false);
                onEncountered();
            }
        }
    }
}
