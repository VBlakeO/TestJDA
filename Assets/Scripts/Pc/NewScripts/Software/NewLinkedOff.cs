using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NewLinkedOff : BasePage
{
    #region Singleton
    public static NewLinkedOff Instance;
    #endregion
    [Space]

    [SerializeField] private GameObject loadScreen;
    [SerializeField] private TextMeshProUGUI loadText;
    [Space]

    public SaveManager saveManager = null;
    public EmployeeManager employeeManager = null;
    public PopUp_Mini popUpMini = null;
    [Space]

    public Image playerImage;
    public Sprite[] playerSprites = null;
    public TranslateTool translateTool;

    [HideInInspector]
    public float playerPayment = 0;

    private void Awake()
    {
        Instance = this;
        translateTool.ChangeLanguage(GameManager.languageId);
    }

    public override void OpenSoftware()
    {
        base.OpenSoftware();

        playerImage.sprite = playerSprites[SavableGameData.selectedCharacter];
    }
    
    public void DismissEmployee(int employeeId)
    {
        for (int i = 0; i < saveManager.employeeList.Count; i++)
        {
            if (saveManager.employeeList[i].id == employeeId)
            {
                saveManager.employeeList[i].GoAway();
                employeeManager.OnNumberOfEmployeesChange.Invoke(employeeManager.GetNumberOfEmployees());
            }
        }
    }

    public void SetLoadScreenState(bool state)
    {
        loadScreen.SetActive(state);
    }

    public void PlayerPayment()
    {
        string warning = translateTool.GetText(0); 
        SavableGameData.DepositMoney(playerPayment);
        ReputationSystem.m_Instance.ApplyReputation(ReputationSystem.m_Instance.reputationInfo.reputationPerGame);
        popUpMini.CallWarningMessage(warning, 1);
    }
}