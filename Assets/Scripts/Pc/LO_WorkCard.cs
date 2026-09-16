using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LO_WorkCard : MonoBehaviour
{
    [SerializeField] private string studioName = "";
    [SerializeField] private float playerPayment = 1500f;
    [SerializeField] private float necessaryProgress = 100f;

    [SerializeField] private Button workButton;
    [SerializeField] private TextMeshProUGUI studioNameText;
    [SerializeField] private TextMeshProUGUI playerPaymentText;

    [SerializeField] private NewLinkedOff linkedOf = null;
    [SerializeField] private Programming programming = null;
    [SerializeField] private GameManager gameManager = null;

    [SerializeField] private TranslateTool translateTool = null;

    void Start()
    {
        translateTool.ChangeLanguage(GameManager.languageId);

        studioNameText.text = studioName;
        playerPaymentText.text = "$" + playerPayment.ToString();

        workButton.onClick.AddListener(ChooseExternalWork);
    }

    public void ChooseExternalWork()
    {
        bool callEvent = true;
        string warning = translateTool.GetText(0).Replace("$ssKey", studioName) + playerPayment.ToString();


        if (programming.externalWorkInProgress)
        {
            callEvent = false;
            warning = translateTool.GetText(1);
            linkedOf.popUpMini.CallWarningMessage(warning, 0);
        }

        List<bool> gamesReady = SavableGameData.gameReady;
        if (gamesReady.Count > 0 && !gamesReady[gamesReady.Count - 1])
        {
            callEvent = false;
            warning = translateTool.GetText(2).Replace("$ssKey", SavableGameData.gameName[SavableGameData.gameName.Count - 1]);
            linkedOf.popUpMini.CallWarningMessage(warning, 0);
        }

        if (callEvent)
        {
            linkedOf.popUpMini.action += DoExternalWork;
            linkedOf.popUpMini.CallWarningMessage(warning, 1);
            linkedOf.playerPayment = playerPayment;
        }
    }

    private void DoExternalWork()
    {
        linkedOf.pc_Manager.OpenSoftware(1);
        programming.externalWorkInProgress = true;

        programming.NewGame(studioName, programming.minimumProgressNeeded + necessaryProgress);
    }
}