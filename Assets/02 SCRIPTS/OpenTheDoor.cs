using System.Collections;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    [SerializeField] private float openSpeed = 5f;
    [SerializeField] private float openDistance = 3f; 
    private Vector3 closedPos;
    private Vector3 openedPos;
    private bool isOpening = false; 
    private Coroutine doorRoutine;

    private void Awake()
    {
        closedPos = transform.position;
        openedPos = closedPos + Vector3.up * openDistance;
    }
    public void StartOpenDoor()
    {
        if (doorRoutine != null)
        {
            StopCoroutine(doorRoutine);
        }
        isOpening = !isOpening;
        doorRoutine = StartCoroutine(DoorCoroutine());
    }
    private IEnumerator DoorCoroutine()
    {
        Vector3 targetPos = isOpening ? openedPos : closedPos;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, openSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        doorRoutine = null;
    }
}