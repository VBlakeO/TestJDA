using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class GetButtonInfo : Button
{
    [Header("Info")]
    public GameObject infoBackground = null;
    public TextMeshProUGUI infoText = null;
    public Vector3 offset = new(0,0,0);
    public TranslateTool translateTool = null;

    protected override void Start() 
    {
        base.Start();
        translateTool.ChangeLanguage(GameManager.languageId);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        infoBackground.GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,0.5f);
        infoBackground.SetActive(true);
        infoText.text = translateTool.GetText(0);

        infoBackground.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + offset;
        infoBackground.GetComponent<RectTransform>().DOScale(Vector3.one, 0.3f);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        infoBackground.GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,0.5f);
        infoBackground.SetActive(false);
    } 
}
