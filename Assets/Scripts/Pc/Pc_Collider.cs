using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class Pc_Collider : MonoBehaviour, I_Interact
{
    [SerializeField] private PlayerMovement player = null;
    [SerializeField] private Transform playerPositionOnPC = null;
    [Space]
    
    [SerializeField] private GameObject pcCam = null;
    [SerializeField] private Camera mainCamera = null;
    [Space]
    
    public bool go = false;
    public Transform camPoint = null;
    public BoxCollider m_Collider = null;
    [Space]

    [SerializeField] private Animator loginAnin = null;
    [SerializeField] private Animator tabletLoginAnin = null;
    [SerializeField] private TutorialManager tutorialManager = null;
    
    [HideInInspector] public Vector3 camRotation = new(11.212f, 0f, 0);

    public void Start()
    {
        loginAnin.speed = 0f;
        tabletLoginAnin.speed = 2f;
        pcCam.transform.position = camPoint.position;
    }

    public void Interact()
    {
        if (!PlayerMovement.Instance.IsGrounded())
            return;

        PlayerMovement.Instance.PausePlayer(true);
        PlayerMovement.Instance.ActivatePlayerControl(true);
        player.agent.SetDestination(playerPositionOnPC.position);
        Hud_Manager.m_Instance.ActiveAim(false);

        loginAnin.speed = 0.85f;
        tabletLoginAnin.speed = 0.85f;

        tutorialManager.NewMessage(1);

        go = true;
        m_Collider.enabled = true;
    }

    public void Release()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !go)
            return;
        
        StartCoroutine(Rotina());
    }

    public IEnumerator Rotina()
    {
        WaitForSeconds wfs = new (0.15f);
        yield return wfs;

        if (go)
        {
            mainCamera.enabled = false;

            pcCam.SetActive(true);
            pcCam.transform.SetPositionAndRotation(player.cameraFps.transform.position, player.cameraFps.transform.rotation);

            iTween.MoveTo(pcCam, camPoint.position, 1.2f);
            iTween.RotateTo(pcCam, camRotation, 1.2f);

            PC_Manager.Instance.on_PC = true;
            m_Collider.enabled = false;

            go = false;
        }
    }
}
