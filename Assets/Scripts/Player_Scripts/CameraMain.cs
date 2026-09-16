using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform[] lookPoint;
    [SerializeField] private bool autoMove = false;
    [SerializeField] private float autoMoveDirection = 1f;




    [Header("Movement")]
    public float moveSmoothTime = 0.30f;
    private Vector3 velocity = Vector3.zero;
    public Vector2 currentDir = Vector2.zero;     
    private Vector2 targetDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private CharacterController controller = null;

    [Header("Slope")]
    private float slopeForce = 5.0f;
    private float slopeForceRayLength = 2.0f;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {

        if (PC_Manager.Instance.on_PC)
            return;
        
        targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        if (autoMove)
        {
            targetDir = new Vector2(autoMoveDirection, Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();
        }


        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed;

        controller.Move(velocity * Time.deltaTime);

        if ((Mathf.Abs(targetDir.x) > 0 || Mathf.Abs(targetDir.y) > 0) && OnSlope())
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
    
        transform.LookAt(new Vector3(transform.rotation.x, lookPoint[0].position.y, lookPoint[0].position.z));
    }

    bool OnSlope()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;

        return false;
    }
}
