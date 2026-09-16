using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField] float duration = 3f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

}
