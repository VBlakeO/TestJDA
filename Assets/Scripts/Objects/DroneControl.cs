using UnityEngine;

public class DroneControl : MonoBehaviour, I_Interact
{
    [SerializeField] private DroneController drone = null;
    [SerializeField] private float nFov = 30.0f;
    
    private bool connected = false;
    private float saveFov = 60.0f;

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!connected)
            {
                connected = true;
                ActivateDrone();
            }
        }
    }

    public void ActivateDrone()
    {
        PlayerMovement.Instance.SetBusyState(true);
        PlayerMovement.Instance.PausePlayer(true);
        
        drone.Connect();
        drone.SetRbDrag(10.0f);

        saveFov = PlayerMovement.Instance.cameraFps.fieldOfView;
        Camera.main.fieldOfView = nFov;
    }

    public void Release()
    {
        drone.SetRbDrag(1.0f);
        drone.Disconnect();
        connected = false;

        PlayerMovement.Instance.cameraFps.fieldOfView = saveFov;

        PlayerMovement.Instance.SetBusyState(false);
        PlayerMovement.Instance.PausePlayer(false);
    }
}
