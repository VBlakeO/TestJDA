using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;

    public static UnityAction<int> OnLanguageChanged = null;
    public static UnityAction OnQuitApplication = null;

    [Header("Localization")]
    [Range(0,1)] public int LanguageId = 0;
    public static int languageId = 0;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        m_Instance = this;
    }

    private void Start() 
    {
        LanguageId = languageId;
        OnLanguageChanged?.Invoke(languageId);
    }

    private void OnEnable()
    {
        OnLanguageChanged?.Invoke(languageId);
    }

    public void ChangeLanguage(int index)
    {
        languageId = index;
        OnLanguageChanged?.Invoke(index);
        print("ChangeLanguage " + index);
    }    
    
    public void SwitchLanguage()
    {
        languageId = languageId == 0? 1 : 0;
        OnLanguageChanged?.Invoke(languageId);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit() 
    {
        OnQuitApplication?.Invoke();    
    }

    private void OnValidate() {
      // languageId = LanguageId;
    }
}