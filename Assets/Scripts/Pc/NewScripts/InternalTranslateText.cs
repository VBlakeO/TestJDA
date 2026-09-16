using UnityEngine;

[System.Serializable]
public class TranslateString
{
    public string stringBase;
    public string[] translateStrings;
}

public class InternalTranslateText : MonoBehaviour
{
    public TranslateString[] TranslateStrings;
    public int currentLanguage = 0;

    private void Start()
    {
        ChangeLanguage(GameManager.languageId);
    }

    public void ChangeLanguage(int _currentLanguage)
    {
        foreach (var translateString in TranslateStrings)
            translateString.stringBase = translateString.translateStrings[_currentLanguage];
    }

    public string GetText(int index)
    {
        return TranslateStrings[index].stringBase;
    }
}
