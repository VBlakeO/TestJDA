using UnityEngine;

public class DroneRing : MonoBehaviour
{
    [SerializeField] private DroneRingGame droneRingGame = null;
    [HideInInspector] public AudioSource audioS = null;

    private void Start() 
    {
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Drone"))
        {
            droneRingGame.AddScore();

            if (audioS)
                audioS.PlayOneShot(audioS.clip);

            Invoke(nameof(PlayEffect), 1.1f);
        }
    }

    private void PlayEffect()
    {
        gameObject.SetActive(false);
    }
}
