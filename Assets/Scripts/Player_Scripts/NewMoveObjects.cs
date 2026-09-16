using UnityEngine;

public class NewMoveObjects : MonoBehaviour
{
    public static NewMoveObjects Instance = null;
    public bool cantInteract = false;
    public LayerMask layerMask = 2;

    [Header("Pickup Settings")]
    [SerializeField] Transform mainCamera = null;
    [SerializeField] Transform holdArea = null;
    private GameObject heldObj = null;
    private Rigidbody heldObjRB = null;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float springConstant = 25.0f;
    
    [Header("Throwing")]
    public float maxThrowingStrength = 300f;
    public float minThrowingStrength = 100f;
    public float throwingStrength = 100f;
    public float throChargeSpeed = 400f;
    public float rotZForce = 100f;

    [Header("Rotation")]
    float rotX, rotY, rotZ;

    public Transform normalHoldArea = null;
    public Transform specialHoldArea = null;
    public Transform droneControlHoldArea = null;
    
    private I_Interact interactiveObj  = null;

    private bool droneControl = false;

    public float distance = 0f;

    private bool CanInteract()
    {
        if (PC_Manager.Instance.on_PC || Time.timeScale <= 0 || cantInteract || !PlayerMovement.Instance.IsGrounded())
            return false;
        else
            return true;
    }

    public bool MovingObject() => heldObj;

    public GameObject GetHeldObj() => heldObj;

    public bool MovingInteractObject() => heldObj && heldObj.transform.GetComponent<I_Interact>() != null;

    private void Awake() => Instance = this;


    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickupRange, layerMask, QueryTriggerInteraction.Ignore) && CanInteract())
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && heldObj)
        {
            DropObject();
        }


        if (Input.GetKey(KeyCode.Mouse1) && heldObj)
        {
            float value = throwingStrength + Time.deltaTime * throChargeSpeed;
            throwingStrength = Mathf.Clamp(value, minThrowingStrength, maxThrowingStrength + 20);
            if (value >= maxThrowingStrength + 20)
            {
                throwingStrength = minThrowingStrength;
            }
            
            Hud_Manager.m_Instance.ThrowingStrength(throwingStrength/maxThrowingStrength);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && heldObj)
        {
           ThrowObject();
        }


        if (Input.GetKey(KeyCode.E) && heldObj)
        {
            if (interactiveObj != null)
            {   
                interactiveObj.Interact();
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Escape) && heldObj)
        {
            if (interactiveObj != null)
            {   
                interactiveObj.Release();
            }
        }


        if (heldObj)
        {
            distance = Vector3.Distance(transform.position, heldObj.transform.position);

            if (distance > 3f)
                DropObject();

            MoveObject();
            RotateObject();
            
            if (droneControl)
                heldObj.transform.localEulerAngles = new (-70.0f, 0f, 180.0f);
        }
    }

    private void RotateObject()
    {
        if (PC_Manager.Instance.on_PC)
            return;

        if (droneControl)
            return;

        if (!PlayerMovement.Instance.busy)
            PlayerMovement.Instance.cantLook = Input.GetKey(KeyCode.R);


        if (Input.GetKey(KeyCode.R))
        {
            rotX = Input.GetAxis("Mouse X") * 5.0f;
            rotY = Input.GetAxis("Mouse Y") * 5.0f;
            rotZ = Input.GetAxis("Mouse ScrollWheel") * 50.0f;

            if(heldObj)
            {
                heldObj.transform.Rotate(mainCamera.up, -rotX, Space.World);
                heldObj.transform.Rotate(mainCamera.right, rotY, Space.World);
                heldObj.transform.Rotate(mainCamera.forward, rotZ, Space.World);
            }
        }
    }

    
    private void MoveObject()
    {
        if(!heldObj)
            return;
            
        Vector3 moveDirection = holdArea.position - heldObj.transform.position;
        Vector3 springForce = springConstant * moveDirection;
        heldObjRB.AddForce(springForce);

        float maxSpeed = 5.0f;
        heldObjRB.linearVelocity = Vector3.ClampMagnitude(heldObjRB.linearVelocity, maxSpeed);
    }

    private void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            pickObj.layer = LayerMask.NameToLayer("HoldInteractive");

            if (pickObj.GetComponentInChildren<DroneControl>())
            {
                droneControl = true;
                holdArea.position = droneControlHoldArea.position;
                pickObj.transform.localEulerAngles = new(-70.0f, 0f, 180.0f);
            }
            else if (pickObj.GetComponentInChildren<SpecialObject>())
                holdArea.localPosition = specialHoldArea.localPosition + pickObj.GetComponentInChildren<SpecialObject>().offset;
            else
                holdArea.position = normalHoldArea.position;

            interactiveObj = pickObj.GetComponent<I_Interact>();

            heldObjRB = pickObj.GetComponent<Rigidbody>();

            if (pickObj.GetComponent<Employee_AI>())
            {
                var employee_AI = pickObj.GetComponent<Employee_AI>();
                employee_AI.ToWakeUp();
                employee_AI.DisableAgent();

                heldObjRB.isKinematic = false;
            }

            heldObjRB.useGravity = false;
            heldObjRB.linearDamping = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    private void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.linearDamping = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        throwingStrength = minThrowingStrength;
        Hud_Manager.m_Instance.ThrowingStrength(0f);

        PlayerMovement.Instance.cantLook = false;
        distance = 0f;

        interactiveObj?.Release();

        if (heldObj.transform.GetComponent<AmazoniaItemWithParent>())
            heldObj.layer = LayerMask.NameToLayer("Drone");
        else
            heldObj.layer = LayerMask.NameToLayer("Interactive");

        heldObjRB.transform.parent = null;
        interactiveObj = null;
        droneControl = false;
        heldObj = null;
    }

    private void ThrowObject()
    {
        Vector3 rayEndPoint = transform.position + transform.TransformDirection(Vector3.forward);
        Vector3  tempDirection = rayEndPoint - transform.position;
        tempDirection.Normalize();

        heldObjRB.useGravity = true;
        heldObjRB.linearDamping = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObjRB.AddForce(tempDirection * throwingStrength);
        throwingStrength = minThrowingStrength;
        Hud_Manager.m_Instance.ThrowingStrength(0f);

        PlayerMovement.Instance.cantLook = false;

        if (interactiveObj != null)
            interactiveObj.Release();

        if (heldObj.transform.GetComponent<AmazoniaItemWithParent>())
            heldObj.layer = LayerMask.NameToLayer("Drone");
        else
            heldObj.layer = LayerMask.NameToLayer("Interactive");

        heldObjRB.transform.parent = null;
        interactiveObj = null;
        droneControl = false;
        heldObj = null;
    }
}
