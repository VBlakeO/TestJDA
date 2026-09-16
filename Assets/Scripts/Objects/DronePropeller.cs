using UnityEngine;

public class DronePropeller : MonoBehaviour
{
    public bool isEnabled = false;
    [Space]

    [SerializeField] private Vector3 direction = Vector3.zero;
    [SerializeField] private GameObject objectTarget = null;

    public void SetState(bool state) => isEnabled = state;
    
    private void Start()
    {
        if (objectTarget == null)
            objectTarget = gameObject;
    }

    private void FixedUpdate()
    {
        if (isEnabled)
            transform.Rotate(direction * Time.deltaTime);
    }
}
