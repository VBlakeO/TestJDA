using UnityEngine;
using System;

public class PC_Manager : MonoBehaviour
{
    public static PC_Manager Instance;

    public Action OnClosePages;
    public Action OnStartProject;
    public Action OnFinishProject;
    [Space]

    [SerializeField] private BasePage[] software = null;
    [SerializeField] private GameObject[] cameras = null;
    [SerializeField] private Transform[] initialMenuAnchors = null;
    [Space]

    [SerializeField] private Canvas pcCanvas = null;
    [SerializeField] private GameObject initialMenu = null;
    [Space]

    [SerializeField] private Hud_Manager hudManager = null;
    [SerializeField] private Pc_Collider pcCollider = null;
    [SerializeField] private PlayerMovement playerMovement = null;

    private int currentCameraId = 1;
    private bool initialMenuState = false;


    [HideInInspector] 
    public bool on_PC = false;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenSoftware(int softwareId)
    {
        CloseAllSoftware();
        software[softwareId].SoftwareScreen.SetActive(true);
        software[softwareId].OpenSoftware();
    }

    private void CloseAllSoftware()
    {
        foreach (var _software in software)
        {
            _software.SoftwareScreen.SetActive(false);
            _software.CloseSoftware();
        }

        OnClosePages?.Invoke();
    }

    public void StartProject()
    {
        OnStartProject?.Invoke();
    }

    public void FinishedProject()
    {
        OnFinishProject?.Invoke();
    }

    public void ReactivatePlayer()
    {
        currentCameraId = 1;

        playerMovement.PausePlayer(false);
        playerMovement.ActivatePlayerControl(false);

        pcCollider.m_Collider.enabled = false;
        pcCollider.StopCoroutine(pcCollider.Rotina());
        pcCollider.go = false;

        cameras[1].SetActive(false);
        cameras[1].transform.SetPositionAndRotation(pcCollider.camPoint.position, Quaternion.Euler(pcCollider.camRotation));

        hudManager.ActiveAim(true);
        pcCanvas.worldCamera = cameras[1].GetComponent<Camera>();

        on_PC = false;
    }

    public void ChangeCamera()
    {
        currentCameraId = currentCameraId == 0 ? 1 : 0;

        foreach (GameObject camera in cameras)
            camera.SetActive(false);

        cameras[currentCameraId].SetActive(true);
        pcCanvas.worldCamera = cameras[currentCameraId].GetComponent<Camera>();
    }

    public void ReativeCurrenteCamera()
    {
        foreach (GameObject camera in cameras)
            camera.SetActive(false);

        cameras[currentCameraId].SetActive(true);
        pcCanvas.worldCamera = cameras[currentCameraId].GetComponent<Camera>();
    }

    public void OpenInitialMenu()
    {
        initialMenuState = !initialMenuState;

        if (initialMenuState)
            iTween.MoveTo(initialMenu, initialMenuAnchors[0].transform.position, 1.15f);
        else
            iTween.MoveTo(initialMenu, initialMenuAnchors[1].transform.position, 0.3f);
    }
}