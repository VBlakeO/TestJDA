using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SellPanel : MonoBehaviour
{
    public GameObject sellPanel = null;
    public TextMeshProUGUI gameCostText = null;
    public TextMeshProUGUI costTextSellPanel = null;

    public Slider slider;


    public void ResetValue()
    {
        gameCostText.text = "$00";
    }

    public void SetPrice(string message)
    {
        costTextSellPanel.text = message;
    }

    public void AcitveSellPanel(float finalValue)
    {
        slider.minValue = finalValue - 30;
        if (slider.minValue == 0)
            slider.minValue = 0;
        slider.maxValue = finalValue + 30;
        slider.value = finalValue;
    }

}
