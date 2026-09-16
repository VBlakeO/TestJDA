using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    public float range = 3;
    public LayerMask layer = 3;
    public bool canInteract = true;

    public NewMoveObjects moveObjects = null;
    public bool movingObject;

    [SerializeField] private GameObject pc = null;
    [SerializeField] private GameObject radio = null;
    
    bool CanInteract()
    {
        if (PC_Manager.Instance.on_PC || Time.timeScale <= 0 || !canInteract)
            return false;
        else
            return true;
    }


    void Update()
    {
        Hud_Manager.m_Instance.ActiveRotateInfo(moveObjects.MovingObject());

        bool centerObj = Physics.Raycast(transform.position, transform.forward, out RaycastHit _hit, range, layer, QueryTriggerInteraction.Ignore) && CanInteract() && !moveObjects.MovingObject();
        Hud_Manager.m_Instance.ActiveAimCircule(centerObj);
        
        Hud_Manager.m_Instance.ActiveCatch(centerObj && _hit.transform.gameObject != pc && _hit.transform.gameObject != radio);
        
        Hud_Manager.m_Instance.ActiveInteractiveInfo(moveObjects.MovingInteractObject() || centerObj && (_hit.transform.gameObject == pc || _hit.transform.gameObject == radio));

        movingObject = moveObjects.MovingInteractObject();

        if (Input.GetKey(KeyCode.E))
        {
            if (!CanInteract())
                return;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, layer, QueryTriggerInteraction.Ignore))
            {   
                if (hit.transform.TryGetComponent<I_Interact>(out var obj))
                {
                    if (hit.transform.GetComponent<Fixo>() && !moveObjects.MovingObject())
                    {
                        obj.Interact();
                    }
                }
            }
        }
    }
}
