using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TempMarketingKing : BasePage
{
    [Space]
    [Header("UI")]
    [SerializeField] private GameObject informationScreen = null;
    [SerializeField] private TextMeshProUGUI disclosureText = null;
    [SerializeField] private TextMeshProUGUI costText = null;
    
    [Space]

    [SerializeField] private Image informationIcon = null;
    [SerializeField] private TextMeshProUGUI informationName = null;
    [SerializeField] private TextMeshProUGUI informationDescription = null;
    [Space]

    public Button[] marketingButtons = null;
    public Toggle[] marketingToggle = null; // Visual indicators of marketing strategy state.
    [Space]


    [Header("References")]
    [SerializeField] private PopUp_Mini warningPopUp = null;

    [Header("Internal")]
    public MarketingStrategy[] marketingStrategy = null; // All marketing strategys.
    [Space]

    [SerializeField] private TranslateTool translateTool = null;

    private int _index = 0;
    private MarketingStrategy _selectedMS = null;

    protected override void Start()
    {
        base.Start();

        translateTool.ChangeLanguage(GameManager.languageId);
        LoadHiredMarketingStrategy();
    }

    public void LoadHiredMarketingStrategy()
    {
        for (int i = 0; i < SavableGameData.hiredMarketing.Length; i++)
        {
            marketingButtons[i].interactable = !SavableGameData.hiredMarketing[i];
            marketingToggle[i].gameObject.SetActive(SavableGameData.hiredMarketing[i]);
            marketingToggle[i].isOn = SavableGameData.activatedMarketing[i];
        }
    }

    public void OpenMarketingStrategyInfo(int index)
    {
        _index = index;
        _selectedMS = marketingStrategy[index];

        informationScreen.SetActive(true);

        disclosureText.text = translateTool.GetText(0) + " " + _selectedMS.disclosure;
        costText.text = translateTool.GetText(1) + _selectedMS.cost;

        informationIcon.sprite = _selectedMS.image;
        informationName.text = _selectedMS.namme[GameManager.languageId];
        informationDescription.text = _selectedMS.description[translateTool.CurrentLanguage];
    }

    public void BuyMarketingStrategy()
    {
        float cost = _selectedMS.cost;
        string namme = _selectedMS.namme[GameManager.languageId];
        string warnigMessage = translateTool.GetText(2);

        if (SavableGameData.CurrentMoney >= cost)
        {
            warnigMessage = warnigMessage.Replace("&0", namme);
            warnigMessage = warnigMessage.Replace("&1", "$" + cost.ToString());
            warningPopUp.SetButtonText(translateTool.GetText(4));
        }
        else
        {
            warnigMessage = translateTool.GetText(3);
            warningPopUp.SetButtonText("Ok");
        }

        warningPopUp.CallWarningMessage(warnigMessage, 0);
    }

    public void ConfirmMarketingStrategyPurchase()
    {
        if (SavableGameData.CurrentMoney >= _selectedMS.cost)
        {
            SavableGameData.WithdrawMoney(_selectedMS.cost);

            SavableGameData.hiredMarketing[_index] = true;
            SavableGameData.activatedMarketing[_index] = true;

            marketingButtons[_index].interactable = false;
            marketingToggle[_index].gameObject.SetActive(true);
            marketingToggle[_index].isOn = true;

            informationScreen.SetActive(false);
            _selectedMS = null;
        }

        warningPopUp.SetPopUpState(false);
    }

    public void SetMarketingStrategyState(int id)
    {
        SavableGameData.activatedMarketing[id] = marketingToggle[id].isOn;
    }

    public float GetMarketingPrice()
    {   
        float marketingPrice = 0f;

        for (int i = 0; i < SavableGameData.activatedMarketing.Length; i++)
        {
            if (SavableGameData.activatedMarketing[i] == true)
                marketingPrice += marketingStrategy[i].cost; 
        }

        return marketingPrice;
    }

    public override void CloseSoftware()
    {
        base.CloseSoftware();
        informationScreen.SetActive(false);
    }
}


// {
//     [Space]
//     public GameObject informationScreen = null;
//     [Space]

//     public Image informationIcon = null;
//     public TextMeshProUGUI informationName = null;
//     public TextMeshProUGUI informationDescription = null;
//     [Space]

//     public TextMeshProUGUI costText = null;
//     public TextMeshProUGUI disclosureText = null;
//     [Space]

//     public GameManager gameManager = null;
//     public PopUp_Mini warningPopUp = null;
//     [Space]

//     public MarketingStrategy[] marketingStrategy = null;
//     public Button[] marketingButtons;
//     public Toggle[] marketingToggle;
//     [Space]
//     [SerializeField] private TranslateTool translateTool = null;

//     private int _index = 0;
//     private int _toggleIndex = 0;

//     protected override void Start()
//     {
//         base.Start();
        
//         gameManager.OnLanguageChanged += translateTool.ChangeLanguage;
//         translateTool.ChangeLanguage(gameManager.LanguageId);

//         LoadHiredMarketingStrategy();
//     }

//     public void OpenMarketingStrategyInfo(int index)
//     {
//         _index = index;

//         informationScreen.SetActive(true);

//         disclosureText.text = translateTool.GetText(0) + " " + marketingStrategy[index].disclosure;
//         costText.text = translateTool.GetText(1) + marketingStrategy[index].cost;      

//         informationIcon.sprite = marketingStrategy[index].image;
//         informationName.text = marketingStrategy[_index].namme[translateTool.CurrentLanguage];
//         informationDescription.text = marketingStrategy[_index].description[translateTool.CurrentLanguage];
//     }

//     public void BuyMarketingStrategy()
//     {
//         float cost = marketingStrategy[_index].cost;
//         string namme = marketingStrategy[_index].namme[translateTool.CurrentLanguage];

//         string warnigMessage = translateTool.GetText(2);

//         if (SavableGameData.CurrentMoney >= marketingStrategy[_index].cost)
//         {
//             warnigMessage = warnigMessage.Replace("&0", namme);
//             warnigMessage = warnigMessage.Replace("&1", "$" + cost.ToString());
//             warningPopUp.SetButtonText(translateTool.GetText(4));
//         }
//         else
//         {
//             warnigMessage = translateTool.GetText(3);
//             warningPopUp.SetButtonText("Ok");
//         }

//         warningPopUp.CallWarningMessage(warnigMessage, 0);
//     }

//     public void ConfirmMarketingStrategyPurchase()
//     {
//         if (SavableGameData.CurrentMoney >= marketingStrategy[_index].cost)
//         {
//             SavableGameData.WithdrawMoney(marketingStrategy[_index].cost);
            
//             SavableGameData.hiredMarketing[_index] = true;
//             SavableGameData.activatedMarketing[_index] = true;

//             marketingButtons[_index].interactable = false;
//             marketingToggle[_index].gameObject.SetActive(true);
//             marketingToggle[_index].isOn = true;

//             informationScreen.SetActive(false);
//         }

//         warningPopUp.SetPopUpState(false);
      
//         print("Hire");
//     }


//     public void LoadHiredMarketingStrategy()
//     {
//         for (int i = 0; i < SavableGameData.hiredMarketing.Length; i++)
//         {
//             marketingButtons[i].interactable = !SavableGameData.hiredMarketing[i];
//             marketingToggle[i].gameObject.SetActive(SavableGameData.hiredMarketing[i]);
//             marketingToggle[i].isOn = SavableGameData.activatedMarketing[i];
//         }
//     }


//     public void SetMarketingStrategyState(int id)
//     {
//         SavableGameData.activatedMarketing[id] = marketingToggle[id].isOn;
//     }

//     public override void CloseSoftware()
//     {
//         base.CloseSoftware();
//         informationScreen.SetActive(false);
//     }
// }