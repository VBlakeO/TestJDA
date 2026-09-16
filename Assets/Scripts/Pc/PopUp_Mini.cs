using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class PopUp_Mini : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private GameObject warnigPanel;
    [SerializeField] private Button warnigButton;
    [SerializeField] private Color[] buttonColor;
    [SerializeField] private TextMeshProUGUI buttonText = null;
    [Space]

    [SerializeField] private AudioSource audioSource = null;

    public Action action;


    void Start()
    {
        warnigButton.onClick.AddListener(ButtonAction);
    }

    public void CallWarningMessage(string _warningText, int _buttonColorId)
    {
        //Text
        warningText.text = _warningText;

        //Button
        if(buttonColor.Length > 0)
            warnigButton.GetComponent<Image>().color = buttonColor[_buttonColorId];

        //Panel
        SetPopUpState(true);
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void ButtonAction()
    {
        if (action != null)
            action.Invoke();

        action = null;

        SetPopUpState(false);
    }

    public void SetPopUpState(bool state)
    {
        warnigPanel.SetActive(state);

        if(!state)
            action -= action;
    }

    public void ClosePopUpState()
    {
        warnigPanel.SetActive(false);
        action -= action;
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
