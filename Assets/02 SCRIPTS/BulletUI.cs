using UnityEngine;
using TMPro; 
public class BulletUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bulletText;

    public void BulletDisplay(int currentAmmo)
    {
        bulletText.text = $"Bullet: {currentAmmo}";
    }
}