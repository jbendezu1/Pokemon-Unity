using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Debug = UnityEngine.Debug;


public class Player : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    public bool hasTeleported = false;
    public bool canMove = false;
    private string spriteName;
    public float moveSpeed;
    private Rigidbody2D myRigidBody;

    private GameObject fade;
    private Image fadeImage;
    private Animator fadeAnimator;
    public GameObject decisionBox;

    public VectorValue startingPosition;

    public event Action onEncountered;
    public event Action<Collider2D> OnEnterTrainersView;

    public LayerMask Ocean;
    public LayerMask Foreground;
    public LayerMask Grass;
    public LayerMask interactableLayer;

    public int badges = 0;

    private bool isMoving;
    private bool onMilotic;
    private Vector3 input;

    private Animator animator;
    private SpriteRenderer spriterenderer;
    public Coroutine co;
    private Character character;

    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] Menu menu;

    private void Awake()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        fade = GameObject.Find("Fade");
        //       inventory = new Inventory();
        //       uiInventory.SetInventory(inventory);
    }

    private void Start()
    {
        transform.position = startingPosition.initialValue;
        fadeAnimator = fade.GetComponent<Animator>();
        fadeImage = fade.GetComponent<Image>();
    }

    public void HandleUpdate()
    {
        // Restrict player movement when menu is on and during fades
        if (fadeImage.IsActive())
            canMove = false;
        else if (!fadeImage.IsActive())
            canMove = true;

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // If player is going left/right lock up/down movement
            if (input.x != 0) input.y = 0;

            // Create player movement
            if (input != Vector3.zero && canMove)
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;
                if (isWalkable(targetPosition))
                {
                    StartCoroutine(MovePlayer(targetPosition));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();

        if (Input.GetKeyDown(KeyCode.M))
            StartCoroutine(MenuManager.Instance.ShowMenu(menu));

        if (fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fade Out"))
            fadeAnimator.SetTrigger("FadeIn");
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.green, 0.3f);


        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
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
        OnMoveOver();
    }

    // Prevent player from moving over foreground tiles
    private bool isWalkable(Vector3 targetposition)
    {
        if (Physics2D.OverlapCircle(targetposition, 0.1f, Foreground | interactableLayer) != null)
        {
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

    private void OnMoveOver()
    {
        checkForEncounter();
        CheckIfInTrainersView();
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

    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovL);
        if (collider != null)
        {
            animator.SetBool("isMoving", false);
            OnEnterTrainersView?.Invoke(collider);
        }
    }
    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    }
}