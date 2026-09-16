using UnityEngine;
using TMPro;

public class TranslateText : MonoBehaviour
{
    public TextMeshProUGUI text;

    [TextArea(5, 20)] public string m_EnglishText, m_PortuguesText;

    private void Start()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        GameManager.OnLanguageChanged += ChangeLanguage;
        ChangeLanguage(GameManager.languageId);
    }

    private void ChangeLanguage(int _currentLanguage)
    {
        text.text = _currentLanguage switch
        {
            0 => m_EnglishText,
            1 => m_PortuguesText,
            _ => m_EnglishText,
        };
    }
}
