using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LO_EmployeeCard : MonoBehaviour
{
    [SerializeField] private int employeeId = 0;
    [SerializeField] private string employeeName = null;
    [SerializeField] private string employeeProfession = null;
    private float employeePayment = 0f;
    [Space]
    
    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI professionText = null;
    [SerializeField] private TextMeshProUGUI salaryText = null;
    [SerializeField] private Image employeeFade = null;
    [SerializeField] private Button emploeeButton;
    [SerializeField] private Image emploeeButtonImage;
    [SerializeField] private TextMeshProUGUI emploeeButtonText;
    [Space]

    [SerializeField] private Color[] colors;
    public bool HiredEmployee {get; set;}
    [Space]

    [SerializeField] private PopUp_Mini m_PopUpMini = null;
    [SerializeField] private Programming m_Programming = null;
    [SerializeField] private NewLinkedOff m_NewLinkedOff = null;
    [SerializeField] private EmployeeManager m_EmployeeManager = null;
    [Space]
    [SerializeField] private TranslateTool translateTool = null;

    private GameObject employee = null;


    private void Start()
    {   
        employeePayment = KnowledgeManager.employeePayment[employeeId];
       
        translateTool.ChangeLanguage(GameManager.languageId);
        Translate(GameManager.languageId);
       
        ResetCard();
    }

    private void ResetCard()
    {
        if (!HiredEmployee)
        {
            string buttonText = translateTool.GetText(0);

            nameText.text = employeeName;
            professionText.text = employeeProfession;
            salaryText.text = "$" + employeePayment.ToString();

            emploeeButtonImage.color = colors[1];
            emploeeButtonText.text = buttonText;

            emploeeButton.onClick.RemoveAllListeners();
            emploeeButton.onClick.AddListener(ChooseEmployee);

            employeeFade.enabled = false;
        }
        else
        {
            DisableCard();
        }
    }

    private void Translate(int i)
    {
        //nameText.text = translateTool.GetText(6) + employeeName;
        emploeeButtonText.text = translateTool.GetText(0);
    }

    public void ChooseEmployee()
    {
        string warningReplace = translateTool.GetText(1).Replace("ssKey", employeeName);
        string warning = warningReplace + employeePayment.ToString();

        int buttonColor = 1;

        if (m_EmployeeManager.employees.Count >= 6)
        {
            warning = translateTool.GetText(2);
            buttonColor = 0;
        }
        else
        {
            if (employeePayment > SavableGameData.currentMoney)
            {
                warning = translateTool.GetText(3);
                buttonColor = 0;
            }
        }

        if (buttonColor != 0)
        {
            m_PopUpMini.action += HireEmployee;
        }
        else print ("Bruh");

        m_PopUpMini.CallWarningMessage(warning, buttonColor);
    }

    private void HireEmployee()
    {
        if (employeePayment <= SavableGameData.currentMoney)
        {
            //employee = m_EmployeeManager.HireEmployee(employeeId);

            if (m_Programming.IsProgrammingAllowed())
                employee.GetComponent<Employee>().StartWorking();

            //SaveManager.Instance.employeeList.Add(employee.GetComponent<Employee>());
            
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

        print("Disable");
    }

    public void CallResignationMessage()
    {
        string warning = translateTool.GetText(5).Replace("ssKey", employeeName);
        m_PopUpMini.action -= HireEmployee;
        m_PopUpMini.action += DismissEmployee;

        m_PopUpMini.CallWarningMessage(warning, 0);
    }

    public void DismissEmployee()
    {
        m_NewLinkedOff.DismissEmployee(employeeId);
        HiredEmployee = false;
        ResetCard();
    }

    public void VacateChair(int chair)
    {
        m_EmployeeManager.emptyTables[chair] = false;
    }
}
