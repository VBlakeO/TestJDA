using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // Velocidade do drone
    [SerializeField] private float rotationSpeed = 2f;  // Velocidade de rotação do drone
    [SerializeField] private float climbSpeed = 2f;  // Velocidade de rotação do drone
    [SerializeField] private Camera droneCamera = null; 
    [SerializeField] private GameObject droneBody = null;


    private Vector2 targetDir = Vector2.zero;
    [HideInInspector] public Vector2 currentDir = Vector2.zero;
    [SerializeField] private float moveSmoothTime = 0.30f;
    [SerializeField] private AudioList audioList = null;
    [Space]

    [SerializeField] private DronePropeller[] dronePropellers = null;

    private Vector2 currentDirVelocity = Vector2.zero;
    private Vector3 velocity = Vector3.zero;


    private Rigidbody rb = null;

    private float pitch = 0f;
    private float yaw = 0f;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private float flyInput = 0f;

    private bool connected = false;
    private bool upsideDown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }   

    public void Connect()
    {
        connected = true;
        Hud_Manager.m_Instance.SetDroneUIState(true);

        if(!upsideDown)
            audioList.PlayAudioClip(0);

        yaw = 0;
        pitch = 0;

        DroneRingGame.Instance.SetRingsLayer("Default");
        droneCamera.enabled = true;

        Hud_Manager.m_Instance.LockCursor(true);

        for (int i = 0; i < dronePropellers.Length; i++)
            dronePropellers[i].SetState(true);
    }

    public void Disconnect()
    {
        connected = false;
        Hud_Manager.m_Instance.SetDroneUIState(false);

        audioList.StopAudio();

        droneCamera.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        DroneRingGame.Instance.SetRingsLayer("DroneGool");
        Hud_Manager.m_Instance.LockCursor(false);

        for (int i = 0; i < dronePropellers.Length; i++)
            dronePropellers[i].SetState(false);
    }

    private void Update()
    {
        upsideDown = Vector3.Dot(transform.up, Vector3.up) <= 0.2f;

        if (!connected)
            return;

        targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        

        horizontalInput = Input.GetAxis("FlyHorizontal");
        verticalInput = Input.GetAxis("FlyVertical");
        flyInput = Input.GetAxis("Fly");

        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= rotationSpeed * Input.GetAxis("Mouse Y");

        if (upsideDown)
        {
            if (rb.constraints != RigidbodyConstraints.None)
            {
                rb.constraints = RigidbodyConstraints.None;
                audioList.StopAudio();
            }
        }
        else
        {
            if (rb.constraints != RigidbodyConstraints.FreezeRotation)
                rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }


    private void MouseLook()
    {
        droneBody.transform.localEulerAngles = new Vector3(verticalInput * 15.0f, 0, -horizontalInput * 15.0f);
        transform.localEulerAngles = new Vector3(pitch, yaw, 0f);
    }


    private void Movement2()
    {
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * (flyInput * climbSpeed);

        rb.linearVelocity = velocity;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0 || !connected)
            return;

        if (upsideDown)
        {
            if (rb.linearDamping > 1)
                 SetRbDrag(0f);
            
            return;
        }

        MouseLook();
        Movement2();
    }

    public void SetRbDrag(float drag)
    {
        rb.linearDamping = drag;
    }
}