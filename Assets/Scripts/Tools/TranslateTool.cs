[System.Serializable]
public class TranslateTool
{
    public TranslateString[] TranslateStrings;


    private int currentLanguage = 0;
    public int CurrentLanguage { get => currentLanguage; set => currentLanguage = value; }

    private void Start() 
    {
        //ChangeLanguage(GameManager.languageId);
    }

    public void ChangeLanguage(int _currentLanguage)
    {
        currentLanguage = _currentLanguage;

        foreach (var translateString in TranslateStrings)
            translateString.stringBase = translateString.translateStrings[GameManager.languageId];
    }

    public string GetText(int index)
    {
        return TranslateStrings[index].stringBase;
    }
}

[System.Serializable]
public class TranslateStringClass
{
    private string stringBase;
    public string[] translateStrings = new string[2];
}