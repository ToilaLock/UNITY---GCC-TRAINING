using UnityEngine;
using UnityEngine.InputSystem;

public class AttackButton : MonoBehaviour
{
    private InputAction attackClick;
    private float attack;
    private void Awake() {
        attackClick = InputSystem.actions.FindAction("Click");
    }

    void Update()
    {
        attack = attackClick.ReadValue<float>();
        if (attackClick.WasPressedThisFrame())
        {
            Debug.Log("Anh Phung Thanh Do");
        }
    }

    private void FixedUpdate() {
        
    }
}
