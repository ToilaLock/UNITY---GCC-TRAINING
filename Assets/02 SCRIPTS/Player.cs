using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float knockBackForce = 1f;
    [SerializeField] private float timeKnockBack = 0.05f;

    [Header("Ground Check")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float castDist;

    [Header("References Anim")]
    [SerializeField] private PlayerAnimation playerAnimation;

    // MOVEMENT & PHYSICS
    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction hitAction;
    private float moveInput;
    private float jumpInput;
    private float hitInput;

    // STATES
    private InteractableObject currentInteractable;
    private int coinCount = 0;
    private bool isKnockBack = false;
    private Coroutine knockBackRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerAnimation == null) playerAnimation = GetComponent<PlayerAnimation>();

        moveAction = InputSystem.actions.FindAction("MovementPlatformer");
        jumpAction = InputSystem.actions.FindAction("MovementPlatformerJump");
        interactAction = InputSystem.actions.FindAction("Interact");
        hitAction = InputSystem.actions.FindAction("Click");
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<float>();
        jumpInput = jumpAction.ReadValue<float>();
        hitInput = hitAction.ReadValue<float>();
        
        //ANIM
        if (playerAnimation != null)
        {
            playerAnimation.UpdateAnimation(moveInput, rb.linearVelocityY, isGrounded(), hitInput);
        }

        // INTERACT
        if (currentInteractable != null && interactAction.WasPressedThisFrame())
        {
            currentInteractable.Interact();
        }
    }

    private void LateUpdate()
    {
        if (isKnockBack) return;

        rb.linearVelocityX = moveInput * speed;

        //Jump
        if (jumpAction.IsPressed() && isGrounded())
        {
            rb.linearVelocityY = jumpInput * jumpForce;
        }
    }

    //GROUND CHECK
    public bool isGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, castDist, groundLayer);
    }

    //INTERACT CHECK
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            Debug.Log($"Da nhat {++coinCount}");
        }
        else if (other.CompareTag("Interactable"))
        {
            if (other.TryGetComponent<InteractableObject>(out var interactableObject))
            {
                currentInteractable = interactableObject;
                Debug.Log("Bam E de tuong tac");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentInteractable = null;
        }
    }

    // KNOCKBACK
    public void shootKnockBack()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 knockBackDirection = ((Vector2)mousePosInWorld - (Vector2)transform.position).normalized;

        if (knockBackRoutine != null) StopCoroutine(knockBackRoutine);
        knockBackRoutine = StartCoroutine(PCoroutine(knockBackDirection));
    }
    private IEnumerator PCoroutine(Vector2 knockBackDirection)
    {
        isKnockBack = true;
        rb.linearVelocity = -knockBackDirection * knockBackForce;
        yield return new WaitForSeconds(timeKnockBack);
        isKnockBack = false;
        knockBackRoutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.down * castDist, boxSize);
    }
}