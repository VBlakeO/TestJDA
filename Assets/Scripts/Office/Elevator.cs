using UnityEngine;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform leftDoor = null;
    [SerializeField] private Transform rightDoor = null;
    [Space]    
    
    [SerializeField] private Transform openedLeftDoor = null;
    [SerializeField] private Transform openedRightDoor = null;
    [Space]

    [SerializeField] private Transform closedLeftDoor = null;
    [SerializeField] private Transform closedRightDoor = null;
    [Space]

    [SerializeField] private AudioList audioList = null;

    [SerializeField] private float duration = 1f;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Employeer") || other.CompareTag("Player"))
            OpenDoors();
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Employeer") || other.CompareTag("Player"))
            CloseDoors();
    }

    private void OpenDoors()
    {
        audioList.PlayOnceAudioClip(0);
        leftDoor.DOMove(openedLeftDoor.position, duration);
        rightDoor.DOMove(openedRightDoor.position, duration);
    }

    private void CloseDoors()
    {
        audioList.PlayOnceAudioClip(0);
        leftDoor.DOMove(closedLeftDoor.position, duration);
        rightDoor.DOMove(closedRightDoor.position, duration);
    }
}
