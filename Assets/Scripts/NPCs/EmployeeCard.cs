using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class EmployeeCard : MonoBehaviour
{
    [SerializeField] private EmployeeInfo employeeInfo = null;
    [Space]

    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI salaryText = null;
    [SerializeField] private TextMeshProUGUI professionText = null;
    [Space]

    [SerializeField] private Image employeeFade = null;
    [Space]
    
    [Header("Button")]
    [SerializeField] private Button emploeeButton;
    [SerializeField] private Color[] colors;
    [Space]

    [SerializeField] private PopUp_Mini popUpMini = null;
    [SerializeField] private NewLinkedOff linkedOff = null;
    [SerializeField] private EmployeeManager employeeManager = null;
    [Space]

    [SerializeField] private TranslateTool translateTool = null;

    private float employeePayment = 0f;
    [HideInInspector] public bool hiredEmployee = false;
    [SerializeField] private Image emploeeButtonImage = null;
    [SerializeField] private TextMeshProUGUI emploeeButtonText = null;

    void Start()
    {
        translateTool.ChangeLanguage(GameManager.languageId);
        employeePayment = KnowledgeManager.employeePayment[employeeInfo.employeeId];

        ResetCard();
    }

    private void ResetCard()
    {
        if (hiredEmployee)
        {
            DisableCard();
            return;
        }

        string buttonText = translateTool.GetText(0);

        nameText.text = employeeInfo.employeeName;
        professionText.text = employeeInfo.employeeProfession;
        salaryText.text = "$" + employeePayment.ToString();

        emploeeButtonImage.color = colors[1];
        emploeeButtonText.text = buttonText;

        emploeeButton.onClick.RemoveAllListeners();
        emploeeButton.onClick.AddListener(ChooseEmployee);

        employeeFade.enabled = false;

    }

    private void ChooseEmployee()
    {
        string warningReplace = translateTool.GetText(1).Replace("ssKey", employeeInfo.employeeName);
        string warning = warningReplace + employeePayment.ToString();

        int buttonColor = 1;

        if (employeeManager.GetNumberOfEmployees() >=  6)
        {
            warning = translateTool.GetText(2);
            buttonColor = 0;
        }
        else if (employeePayment > SavableGameData.currentMoney)
        {
            warning = translateTool.GetText(3);
            buttonColor = 0;
        }

        if (buttonColor != 0)
        {
            popUpMini.action += HireEmployee;
            print("GHj");
        }
        else
            print("Ui");
         

        popUpMini?.CallWarningMessage(warning, buttonColor);
    }

    private void HireEmployee()
    {
        print("Hire");

        if (employeePayment <= SavableGameData.currentMoney)
        {
            employeeManager.HireEmployee(employeeInfo.employeeId);
            DisableCard();
        }
    }

    public void DisableCard()
    {
        string buttonText = translateTool.GetText(4);

        employeeFade.enabled = true;

        emploeeButtonImage.color = colors[0];
        emploeeButtonText.text = buttonText;

        emploeeButton.onClick.RemoveAllListeners();
        emploeeButton.onClick.AddListener(CallResignationMessage);
    }

    private void CallResignationMessage()
    {
        string warning = translateTool.GetText(5).Replace("ssKey", employeeInfo.employeeName);

        popUpMini.action -= HireEmployee;
        popUpMini.action += DismissEmployee;

        popUpMini.CallWarningMessage(warning, 0);
    }

    public void DismissEmployee()
    {
        linkedOff.DismissEmployee(employeeInfo.employeeId);
        hiredEmployee = false;
        ResetCard();
    }
}

[Serializable]
public class EmployeeInfo
{    
    public int employeeId = 0;
    public string employeeName = null;
    public string employeeProfession = null;
}
