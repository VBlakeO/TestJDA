using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameProgressController : MonoBehaviour
{   
    [SerializeField] private float moneyRequired = 23000f;
    [SerializeField] private int numberOfEmployeesRequired = 4;
    [SerializeField] private int activatedMarketingAmountRequired = 3;
    [SerializeField] private int requiredReleasedGames = 12;
    [Space]    
    
    [SerializeField] private float taxValue = 3000f;
    [Space]

    [SerializeField] private GameObject invitationScreen = null;
    [Space]

    [SerializeField] private ReputationSystem reputationSystem = null;
    [SerializeField] private EmployeeManager employeeManager = null;
    [SerializeField] private MarketingKing marketingKing = null;
    [SerializeField] private NewDevOp devOp = null;
    [SerializeField] private InMail inMail = null;
    [Space]

    [SerializeField] private Image moneyProgressBar;
    [SerializeField] private Image releasedGamesProgressBar;
    [SerializeField] private Image employeesRequiredProgressBar;
    [SerializeField] private Image activatedMarketingProgressBar;
    [SerializeField] private Image reputationProgressBar;
    [Space]

    
    [SerializeField] private TextMeshProUGUI moneyProgressText;
    [SerializeField] private TextMeshProUGUI releasedGamesProgressText;
    [SerializeField] private TextMeshProUGUI employeesRequiredProgressText;
    [SerializeField] private TextMeshProUGUI activatedMarketingProgressText;
    [SerializeField] private TextMeshProUGUI reputationProgressText;

    
    private void Start() 
    {
        SaveManager.OnFinishLoad += Initialize;
    }

    private void Initialize()
    {
        ClearBars();

        devOp.OnSellingGame += VerifyProgress;
        devOp.OnSellingGame += CheckPayTax;
        devOp.OnGamesSoldChange += UpdateReleasedGamesBar;

        employeeManager.OnHiring += VerifyProgress;
        employeeManager.OnNumberOfEmployeesChange += UpdateEmployeesBar;

        marketingKing.OnActivated += VerifyProgress;
        marketingKing.OnActivatedMarketingChange += UpdateMarketingBar;

        SavableGameData.OnDepositMoney += VerifyProgress;
        SavableGameData.OnMoneyChange += UpdateMoneyBar;

        reputationSystem.OnMaxReputationIsReached += VerifyProgress;
        reputationSystem.OnReputationGhange +=  UpdateReputationBar;

        
    }

    public bool ObjectivesAchieved()
    {
        return SavableGameData.currentMoney >= moneyRequired &&
            reputationSystem.RequiredReputationAchieved() &&
            marketingKing.GetActivatedMarketingAmount() >= activatedMarketingAmountRequired &&
            employeeManager.GetNumberOfEmployees() >= numberOfEmployeesRequired &&
            SavableGameData.gamePublished.Count >= requiredReleasedGames;
    }

    private void VerifyProgress()
    {
        if (ObjectivesAchieved())
            invitationScreen.SetActive(true);
    }

    private void CheckPayTax()
    {
        if (SavableGameData.gamePublished.Count % 3 == 0)
            PayTax();

        if (SavableGameData.gamePublished.Count == 7)
        {
            //if (Random.Range(0, 100) > 80)
            reputationSystem.RemoveReputation(30f);
            inMail.SendFakeNewsEmail();
        }
    }

    private void PayTax()
    {
        print("PayTax");
        SavableGameData.WithdrawMoney(taxValue);
    }


    private void UpdateMoneyBar(float value)
    {
        moneyProgressBar.fillAmount = value / moneyRequired;
        moneyProgressText.text = value + "/" + moneyRequired;
    }
    
    private void UpdateReleasedGamesBar(int value)
    {  
        releasedGamesProgressBar.fillAmount = (float)value / requiredReleasedGames;
        releasedGamesProgressText.text = value + "/" + requiredReleasedGames;
    }
    
    private void UpdateEmployeesBar(int value)
    {
        employeesRequiredProgressBar.fillAmount = (float)value / numberOfEmployeesRequired;
        employeesRequiredProgressText.text = value + "/" + numberOfEmployeesRequired;
    }
    
    private void UpdateMarketingBar(int value)
    {
        activatedMarketingProgressBar.fillAmount = (float)value / activatedMarketingAmountRequired;
        activatedMarketingProgressText.text = value + "/" + activatedMarketingAmountRequired;
    }
    
    private void UpdateReputationBar(float value)
    {
        reputationProgressBar.fillAmount = value * 0.01f;
        reputationProgressText.text = value + "/" + reputationSystem.ReputationRequired;
    }

    private void ClearBars()
    {
        moneyProgressBar.fillAmount = SavableGameData.currentMoney/moneyRequired;
        releasedGamesProgressBar.fillAmount = (float)SavableGameData.gamePublished.Count / requiredReleasedGames;
        employeesRequiredProgressBar.fillAmount = (float)employeeManager.GetNumberOfEmployees() / numberOfEmployeesRequired;
        activatedMarketingProgressBar.fillAmount = (float) marketingKing.GetActivatedMarketingAmount() / activatedMarketingAmountRequired;
        reputationProgressBar.fillAmount = SavableGameData.reputation / reputationSystem.ReputationRequired;

        moneyProgressText.text = SavableGameData.currentMoney + " / " + moneyRequired;
        releasedGamesProgressText.text =  SavableGameData.gamePublished.Count + " / " + requiredReleasedGames;
        employeesRequiredProgressText.text = employeeManager.GetNumberOfEmployees() + " / " + numberOfEmployeesRequired;
        activatedMarketingProgressText.text = marketingKing.GetActivatedMarketingAmount() + " / " + activatedMarketingAmountRequired;
        reputationProgressText.text = SavableGameData.reputation + " / " + reputationSystem.ReputationRequired;
    }
}
