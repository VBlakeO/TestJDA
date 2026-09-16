using UnityEngine.Animations;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    public static MovementPlayer m_Instance;

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
    public LayerMask layerMask = 2;
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
    private Rigidbody rb = null;
    private NavMeshAgent agent = null;
    private LookAtConstraint lookConstraint = null;

    public bool check;

    private void Awake()
    {
        m_Instance = this;

        lookConstraint = GetComponentInChildren<LookAtConstraint>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        origSpeed = agent.speed;
        linking = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !cantJump)
            Jump();

        Movement();
        Linking();
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 0)
            return;

        if (cantLook)// || PC_Manager.m_Instance.on_PC)
            return;

        Vector2 targetMouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        headPitch -= targetMouseDelta.y * mouseSensitivity;
        headPitch = Mathf.Clamp(headPitch, -85, 75);

        head.localEulerAngles = Vector3.right * headPitch;
        transform.Rotate(mouseSensitivity * targetMouseDelta.x * Vector3.up);
    }


    private void Movement()
    {
        if (cantMove)// || PC_Manager.m_Instance.on_PC)
            return;

        targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (IsGrounded() && !isJumping)
            velocityY = 0;

        velocityY += gravity * Time.deltaTime;
        
        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY;

        if (IsGrounded())
            rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        isJumping = true;
        velocityY = jumpForce;

        StartCoroutine(BackToGround());
    }

    private void Linking()
    {
        if (Time.timeScale == 0)
            return;

        if (agent.isOnOffMeshLink && linking == false)
        {
            linking = true;
            agent.speed *= linkSpeed;
        }
        else if (agent.isOnNavMesh && linking == true)
        {
            linking = false;
            agent.velocity = Vector3.zero;
            agent.speed = origSpeed;
        }
    }


    public void PausePlayer(bool active)
    {
        agent.enabled = active;
        cantLook = active;
        cantMove = active;
        currentDir = Vector2.zero;
        lookConstraint.enabled = active;

        if(active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SetCameraState(true);
        }
    }

    public void SetCameraState(bool state)
    {
        cameraFps.enabled = state;
    }


    private IEnumerator BackToGround()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        yield return wfs;
        isJumping = false;
    }

    private bool IsGrounded()
    {
        return (Physics.SphereCast(transform.position, radius, -transform.up, out RaycastHit hit, range, layerMask));
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - transform.up * range, radius);
    }

    /*
    Refazer a movimentação do Player;

    Programar a MoneyGun;
    Programar o Extintor;
    Programar o Drone;

    Fazer o save dos itens comprados;

    Programar a IA dos NPC's;
    */

}
