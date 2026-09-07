using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;
    private float moveInput;
    private float jumpInput;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("MovementPlatformer");
        jumpAction = InputSystem.actions.FindAction("MovementPlatformerJump");
    }

    private void Update() {
        moveInput = moveAction.ReadValue<float>();
        jumpInput = jumpAction.ReadValue<float>();
    }

    private void LateUpdate() {
        rb.linearVelocityX = moveInput * speed;
        if (jumpAction.WasPressedThisFrame())
        rb.linearVelocityY = jumpInput * jumpForce;
    }
}
