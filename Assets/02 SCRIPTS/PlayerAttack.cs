using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private float comboResetTime = 0.8f;
    [SerializeField] private int comboStepMax = 2;
    private InputAction hitAction;
    private bool attackState = false;
    private Coroutine attack;

    void Awake()
    {
        hitAction = InputSystem.actions.FindAction("Click");
    }

    void Update()
    {
        if(hitAction.WasPressedThisFrame()) {
            if(attack == null) attack = StartCoroutine("startAttack");
        }
    }

    private IEnumerator startAttack()
    {
        attackState = true;
        yield return null;
        int comboStep = 1;
        float attackTimer = 0f;
        bool attackPress = false;
        playerAnimation.UpdateAttackAnim(comboStep);
        
        while (attackTimer <= comboResetTime)
        {
            if (hitAction.WasPressedThisFrame())
            {
                attackPress = true;

            }
            attackTimer += Time.deltaTime;
            yield return null;
        }

        if (attackPress)
        {
            comboStep++;
            playerAnimation.UpdateAttackAnim(comboStep);
            yield return new WaitForSeconds(0.8f);
        }

        comboStep = 0;
        playerAnimation.UpdateAttackAnim(comboStep);
        attackState = false;
        attack = null;
        yield return null;
    }
}
