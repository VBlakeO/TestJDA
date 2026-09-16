using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MarketingKing : BasePage
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

    public UnityAction OnActivated;
    public UnityAction <int>OnActivatedMarketingChange;

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
        informationName.text = _selectedMS.namme[translateTool.CurrentLanguage];
        informationDescription.text = _selectedMS.description[translateTool.CurrentLanguage];
    }

    public void BuyMarketingStrategy()
    {
        float cost = _selectedMS.cost;
        string namme = _selectedMS.namme[translateTool.CurrentLanguage];
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
        OnActivated?.Invoke();
        OnActivatedMarketingChange?.Invoke(GetActivatedMarketingAmount());
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

    public int GetActivatedMarketingAmount()
    {
        int _activatedMarketingAmount = 0;
        foreach (bool _activatedMarketing in SavableGameData.activatedMarketing)
            if (_activatedMarketing)
                _activatedMarketingAmount++;

        return _activatedMarketingAmount;
    }

    public override void CloseSoftware()
    {
        base.CloseSoftware();
        informationScreen.SetActive(false);
    }
}