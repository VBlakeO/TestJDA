using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class InfoButton : Button
{   
    public GameObject info = null;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        EnableInfoState();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        DisableInfoState();
    }

    public void EnableInfoState()
    {
        info.SetActive(true);
    }

    public void DisableInfoState()
    {
        info.SetActive(false);
    }
}
