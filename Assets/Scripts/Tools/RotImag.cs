using UnityEngine;

public class RotImag : MonoBehaviour
{
    public Vector3 direction = new();

	private void FixedUpdate() 
    {
        transform.Rotate(direction, Space.World);
    }
}
