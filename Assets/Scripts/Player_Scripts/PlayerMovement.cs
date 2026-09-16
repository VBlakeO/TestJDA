using UnityEngine.Animations;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("===PlayerMovement===")]
    public bool cantMove = false;
    public bool cantLook = false;
    public bool cantJump = false;

    [Header("Movement")]
    public float speed = 0.30f;
    public float moveSmoothTime = 0.30f;
    private Vector3 velocity = Vector3.zero;
    public Vector2 currentDir = Vector2.zero;     
    private Vector2 targetDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    [Header("===Vision===")]
    public float mouseSensitivity;
    private float headPitch = 0.0f;


    [Header("Jump")]
    public LayerMask layerMask = 1;
    public float jumpForce = 5.0f;
    public float radius = 0.61f;
    public float range = 0.55f;
    public float gravity = -13.0f;
    public bool isJumping = false;
    private float velocityY = 0.0f;

    [Header("Linking")]
    public bool linking = false;
    public float linkSpeed = 0.5f;
    public float origSpeed = 0.0f;

    [Header("Component")]
    public Transform head = null;
    public Camera cameraFps = null;
    private CharacterController controller = null;
    private LookAtConstraint lookConstraint = null;
    [HideInInspector] public NavMeshAgent agent = null;

    [Header("Slope")]
    private float slopeForce = 5.0f;
    private float slopeForceRayLength = 2.0f;

    public bool busy = false;

    //public GameObject CameraMan; 
    private void Awake()
    {
        Instance = this;

        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        lookConstraint = GetComponentInChildren<LookAtConstraint>();

        origSpeed = agent.speed;
        linking = false;
    }

    private void Start() 
    {
        if (PlayerPrefs.HasKey("Sensibility"))
            mouseSensitivity = PlayerPrefs.GetFloat("Sensibility");
        else
            mouseSensitivity = 2f;
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !cantJump)
            Jump();

        if (lookConstraint.enabled)
        {
            float ei = head.localRotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y + ei, 0);
            headPitch = head.transform.localRotation.eulerAngles.x;
        }

        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     CameraMan.SetActive(true);
        //     gameObject.SetActive(false);
        // }

        Movement();
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 0)
            return;

        if (cantLook || PC_Manager.Instance.on_PC)
            return;

        Vector2 targetMouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        headPitch -= targetMouseDelta.y * mouseSensitivity;
        headPitch = Mathf.Clamp(headPitch, -85, 75);

        head.localEulerAngles = Vector3.right * headPitch;
        transform.Rotate(mouseSensitivity * targetMouseDelta.x * Vector3.up);
    }

    private void Movement()
    {
        if (IsGrounded() && !isJumping)
            velocityY = 0;

        velocityY += gravity * Time.deltaTime;

        if (!cantMove)
        {
            if (PC_Manager.Instance.on_PC)
                return;

            targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            if ((Mathf.Abs(targetDir.x) > 0 || Mathf.Abs(targetDir.y) > 0) && OnSlope())
                controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.up * velocityY;
            
            if (controller.enabled)
                controller.Move(velocity * Time.deltaTime);
        }
    }

    private void Jump()
    {
        isJumping = true;
        velocityY = jumpForce;

        StartCoroutine(BackToGround());
    }

    private void Linking()
    {
        if (agent.isOnOffMeshLink && linking == false)
        {
            print("Test B");
            linking = true;
            agent.speed *= linkSpeed;
        }
        else if (agent.isOnOffMeshLink && linking == true)
        {
            print("Test C");
            linking = false;
            agent.velocity = Vector3.zero;
            agent.speed = origSpeed;
        }
    }

    public void ActivatePlayerControl(bool active)
    {
        agent.enabled = active;
        lookConstraint.enabled = active;
        controller.enabled = !active;
    }

    public void PausePlayer(bool active)
    {
        cantLook = active;
        cantMove = active;
        cantJump = active;
        currentDir = Vector2.zero;

        if(active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            cameraFps.transform.localEulerAngles = Vector3.zero;
            SetCameraState(true);
        }
    }

    public void SetCameraState(bool state) => cameraFps.enabled = state;

    public void SetBusyState(bool state) => busy = state;

    private IEnumerator BackToGround()
    {
        WaitForSeconds wfs = new(0.1f);
        yield return wfs;
        isJumping = false;
    }

    bool OnSlope()
    {
        if (isJumping)
            return false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;

        return false;
    }

    public bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, radius, -transform.up, out RaycastHit hit, range, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - transform.up * range, radius);
    }
}