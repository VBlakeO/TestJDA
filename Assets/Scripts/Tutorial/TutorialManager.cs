using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTutorial = null;
    [SerializeField] private Image tutorialBackground = null;
    public bool tutorialCompleted = false;
    [Space]

    [SerializeField] private RectTransform openPosition = null;
    [SerializeField] private RectTransform closePosition = null;
    [Space]

    [SerializeField] private GameObject myStudioExitButton = null;
    [SerializeField] private GameObject toolbarItens = null;
    [Space]
    
    [SerializeField] private TranslateTool translateTool;

    void Start()
    {
        translateTool.ChangeLanguage(GameManager.languageId);

        textTutorial.enabled = false;
        tutorialBackground.enabled = false;

        myStudioExitButton.SetActive(true);
        toolbarItens.SetActive(true);

        if (!SavableGameData.readySelection)
            NewMessage(0);
        else
            tutorialCompleted = true;
    }

    public void NewMessage(int currentMessage)
    {
        if (tutorialCompleted)
            return;

        tutorialBackground.GetComponent<RectTransform>().DOMove(closePosition.position, 0.5f).OnComplete(() => {tutorialBackground.GetComponent<RectTransform>().DOMove(openPosition.position, 0.5f);});

        textTutorial.enabled = true;
        tutorialBackground.enabled = true;

        myStudioExitButton.SetActive(false);
        toolbarItens.SetActive(false);

        textTutorial.text = translateTool.GetText(currentMessage);
    }

    public void CloseMessage()
    {
        tutorialBackground.GetComponent<RectTransform>().DOMove(closePosition.position, 0.5f).OnComplete(() =>
        {
            textTutorial.enabled = false;
            tutorialBackground.enabled = false;

            myStudioExitButton.SetActive(true);
            toolbarItens.SetActive(true);
        });
    }
}
