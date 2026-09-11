using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hp = 36;
    
    public void takeDame(int dame)
    {
        hp -= dame;
        if (hp < 1)
        {
            hp = 0;
            die();
        }

        Debug.Log($"Enemy hp now is {hp}");
    }

    private void die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy Dead");
    }
}
