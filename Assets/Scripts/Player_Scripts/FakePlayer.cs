using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [Header("===Vision===")]
    public float mouseSensitivity = 2f;
    private float headPitch = 0.0f;
    private float headPitchX = 0.0f;

    
    [Header("Component")]
    public Transform head = null;

    private void LateUpdate()
    {
        if (Time.timeScale == 0)
            return;

        Vector2 targetMouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        headPitch -= targetMouseDelta.y * mouseSensitivity;
        headPitch = Mathf.Clamp(headPitch, -85, 75);

        headPitchX -= targetMouseDelta.x * mouseSensitivity;
        headPitchX = Mathf.Clamp(headPitchX, -75, 75);

        head.localEulerAngles = Vector3.right * headPitch + -Vector3.up * headPitchX;
    }

}