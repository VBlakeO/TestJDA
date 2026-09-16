using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Newspaper : MonoBehaviour
{
    [SerializeField] private Image logo = null;
    [SerializeField] private TextMeshProUGUI descriptionText = null;

    public void UpdateInfo(Sprite _logo, string _description)
    {
        logo.sprite = _logo;
        descriptionText.text = _description;
    }
}
