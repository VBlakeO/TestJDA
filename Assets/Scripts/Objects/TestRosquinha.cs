using UnityEngine;

public class TestRosquinha : MonoBehaviour
{
    [SerializeField] private float range = 0.1f;
    [SerializeField] private float wallDistance = 0.05f;

    private Rigidbody rb = null;
    private bool paste = false;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range))
            return;

        paste = true;
        rb.linearDamping = 20f;
        transform.SetPositionAndRotation(hit.point - transform.forward * wallDistance, Quaternion.FromToRotation(-Vector3.forward, hit.normal));

        print("COlou?");
    }

    private void OnCollisionExit(Collision other) 
    {
        if (paste) 
        {
            rb.linearDamping = 1;
            paste = false;
            print("Descolou?");
        }   
    }

}
