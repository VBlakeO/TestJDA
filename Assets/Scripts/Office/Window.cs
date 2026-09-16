using UnityEngine;

public class Window : MonoBehaviour
{   
    [SerializeField] private GameObject glass = null;
    [SerializeField] private GameObject brokenGlass = null;
    [SerializeField] private Rigidbody[] piecesOfGlass = null;
    [SerializeField] private Vector3 impactDirection = Vector3.zero;

    private void OnCollisionEnter(Collision other) {

        if (other.transform.GetComponent<Rigidbody>())
        {
            if (other.transform.GetComponent<Rigidbody>().mass > 1 && other.transform.GetComponent<Rigidbody>().linearVelocity.magnitude > 5f)
            {
                glass.SetActive(false);
                brokenGlass.SetActive(true);

                GetComponent<BoxCollider>().enabled = false;
                
                foreach (Rigidbody rb in piecesOfGlass)
                    rb.AddForce(impactDirection, ForceMode.Impulse);

                Destroy(gameObject, 5f);
            }
        }
    }
}
