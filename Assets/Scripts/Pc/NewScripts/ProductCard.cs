using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ProductCard : MonoBehaviour
{
    public int productId = 0;
    public float productPrice = 0;
    [Space]

    public TextMeshProUGUI productNameText = null;
    [Space]

    public Button myButton = null;
    public PopUp_Mini popUp = null;
    public Amazonia amazonia =null;
    
    void Start()
    {
        myButton.onClick.AddListener(OnClick);
    }

    public void SetButtonState(bool state)
    {
        myButton.interactable = state;
    }

    private void OnClick()
    {
        string warning = "";

        if (SavableGameData.CurrentMoney >= productPrice)
        {
            warning = amazonia.translateTool.GetText(0).Replace("$!1", productNameText.text);
            warning = warning.Replace("$!2", "$" + productPrice.ToString()) + "?";

            popUp.action = null;
            popUp.action += Purchase;
        }
        else
        {
            warning = amazonia.translateTool.GetText(1);
            popUp.ClosePopUpState();
        }

        popUp.CallWarningMessage(warning, 0);
    }

    private void Purchase()
    {
        SavableGameData.WithdrawMoney(productPrice);
        amazonia.BuyItem(productId);
    }
}
