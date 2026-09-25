using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform visualTransform; 

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (visualTransform == null) visualTransform = transform;
    }

    public void UpdateAnimation(float moveInput, float verticalVelocity, bool isGrounded)
    {
        if (animator == null) return;

        animator.SetFloat("isRun", Mathf.Abs(moveInput));
        animator.SetFloat("isJump", verticalVelocity);
        animator.SetBool("onGrounded", isGrounded);

        HandleFlip(moveInput);
    }

    public void UpdateAttackAnim(int step)
    {
        animator.SetInteger("comboCount", step);
    }

    private void HandleFlip(float moveInput)
    {
        Vector3 currentScale = visualTransform.localScale;

        if (moveInput > 0.01f)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }
        else if (moveInput < -0.01f)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
        }

        visualTransform.localScale = currentScale;
    }
}