using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CanvasClick : MonoBehaviour, IPointerClickHandler
{
    public Canvas targetCanvas = null;
    public PC_Manager pcManager = null;
    public AudioList audioList = null;

    private EventSystem eventSystem;
    private GraphicRaycaster raycaster;

    void Start()
    {
        raycaster = targetCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (!pcManager.on_PC)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData pointerEventData = new(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(pointerEventData, results);

            if (results.Count > 0)
            {
                audioList.PlayRandonAudioClip();
                Debug.Log("Clique detectado dentro do Canvas!");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // if (!pcManager.on_PC)
        //     return;

        // PointerEventData pointerEventData = new(eventSystem)
        // {
        //     position = Input.mousePosition
        // };

        // List<RaycastResult> results = new List<RaycastResult>();
        
        // raycaster.Raycast(pointerEventData, results);

        // if (results.Count > 0)
        // {
        //     audioList.PlayRandonAudioClip();
        //     Debug.Log("Clique detectado dentro do Canvas!");
        // }
    }
}
