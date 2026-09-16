using UnityEngine;

public class BasketBall : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private AudioList audioList = null;

    void Start()
    {
        if (BasketballGame.Instance)
            BasketballGame.Instance.AddBallCollider(sphereCollider);

        audioList = GetComponent<AudioList>();
    }

    private void OnCollisionEnter(Collision other) 
    {
        audioList.PlayOnceAudioClip(0);
        audioList.audioSource.volume = other.relativeVelocity.magnitude * 0.1f;
    }
}
