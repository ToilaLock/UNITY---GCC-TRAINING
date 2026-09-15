using System.Collections;
using UnityEngine;

public class LeverAnim : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 120f; 
    [SerializeField] private float targetAngle = 60f;  

    private Quaternion startRot;
    private Quaternion openedRot;
    private bool isActivated = false;
    private Coroutine rotateRoutine;

    private void Awake()
    {
        startRot = transform.rotation;
        openedRot = startRot * Quaternion.Euler(0f, 0f, targetAngle);
    }
    public void ToggleLever()
    {
        if (rotateRoutine != null)
        {
            StopCoroutine(rotateRoutine);
        }

        isActivated = !isActivated;
        rotateRoutine = StartCoroutine(RotateCoroutine());
    }
    private IEnumerator RotateCoroutine()
    {
        Quaternion targetRot = isActivated ? openedRot : startRot;

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRot;
        rotateRoutine = null;
    }
}