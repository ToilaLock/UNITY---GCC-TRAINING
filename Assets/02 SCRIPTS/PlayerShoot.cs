using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    [Header("Bullet Setting")]
    [SerializeField] private BulletFly bullet;
    [SerializeField] private int bulletNum = 7;

    //INPUT SYSTEM
    private InputAction clickAction;
    private float clickInput;
    private int currBullet;

    [Header("Shoot Event")]
    [SerializeField] private UnityEvent<int> bulletChange;

    private void Awake()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        currBullet = bulletNum;
    }

    private void Start()
    {
        bulletChange.Invoke(currBullet);
    }

    private void Update()
    {
        if(clickAction.WasPressedThisFrame() && currBullet > 0)
        {
            currBullet--;
            bulletChange.Invoke(currBullet);
            Debug.Log($"Bullet: {currBullet}");

            Instantiate(bullet, transform.position, quaternion.identity);
            GetComponent<Player>().shootKnockBack();
        }
    }
}
