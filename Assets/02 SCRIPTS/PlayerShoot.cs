using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private BulletFly bullet;
    private InputAction clickAction;
    private float clickInput;

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
    }

    private void Update()
    {
        if(clickAction.WasPressedThisFrame())
        {
            Instantiate(bullet, transform.position, quaternion.identity);
        }
    }
}
