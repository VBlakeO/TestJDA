using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private GameObject settingsMenu = null;
    [SerializeField] private GameObject graphicsMenu = null;
    [SerializeField] private GameObject interfaceMenu = null;
    [SerializeField] private GameObject controlsMenu = null;
    [SerializeField] private GameObject audioMenu = null;
    [Space]

    [SerializeField] private GameObject continueButton = null;
    [SerializeField] private GameObject newGameWarning = null;
    [Space]

    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private Slider musicSlider = null;
    //[SerializeField] private Slider SensibilitySlider = null;
    [Space]
    
    [SerializeField] private TMP_Dropdown resolucaoDropdown = null;
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private TMP_Dropdown languageDropdown = null;
    [SerializeField] private Toggle fullScreenToggle = null;
    [Space]

    [SerializeField] private string[] languageEn = null;
    [SerializeField] private string[] languagePt = null;


    [Space]

    [SerializeField] private bool deletePlayerPrefs = false;

    private bool fullScreenState = false;

    private int currentResolucaoId = 0;
    private int graphicQuality = 0;
    private int currentLanguage = 0;
    private int windowMode = 0;

    private float sfxVolume = 0f;
    private float musicVolume = 0f;

    [SerializeField] private TextMeshProUGUI sensibilityText = null;
    [SerializeField] private float sensibility = 2f;

    [SerializeField] private Vector2[] supportedResolutions;
    private string path = "";


    private void Start() 
    {
        AddResolutions();
        AdjustQualities();
        SetLanguage();

        path = Application.persistentDataPath + "/saves/Game.txt";

        if (PlayerPrefs.HasKey("Resolutions"))
        {
            int resolutionNum = PlayerPrefs.GetInt("Resolutions");
            if (supportedResolutions.Length <= resolutionNum)
                PlayerPrefs.DeleteKey("Resolutions");
        }


        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("SFX_volume"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFX_volume");
            sfxSlider.value = sfxVolume;
            //setar volumes;
        }
        else
        {
            PlayerPrefs.SetFloat("SFX_volume", 0.8f);
            sfxSlider.value = 0.8f;
            //setar volumes;
        }

        if (PlayerPrefs.HasKey("Music_volume"))
        {
            musicVolume = PlayerPrefs.GetFloat("Music_volume");
            musicSlider.value = musicVolume;
            //setar volumes;
        }
        else
        {
            PlayerPrefs.SetFloat("Music_volume", 0.8f);
            musicSlider.value = 0.8f;
            //setar volumes;
        }


        if (PlayerPrefs.HasKey("Sensibility"))
        {
            sensibility = PlayerPrefs.GetFloat("Sensibility");
            sensibilityText.text = sensibility.ToString("F1");
        }
        else
        {
            PlayerPrefs.SetFloat("Sensibility", 2f);
            sensibilityText.text = "2.0";
        }

        //=============MODO JANELA===========//
        if (PlayerPrefs.HasKey("WindowMode"))
        {
            windowMode = PlayerPrefs.GetInt("WindowMode");
            
            if (windowMode == 1)
            {
                fullScreenState = false;
                Screen.fullScreen = false;
                fullScreenToggle.isOn = false;
            }
            else
            {
                fullScreenState = true;
                Screen.fullScreen = true;
                fullScreenToggle.isOn = true;
            }
            
        }
        else
        {
            windowMode = 1;
            PlayerPrefs.SetInt("WindowMode", 1);

            fullScreenState = true;
            Screen.fullScreen = true;
            fullScreenToggle.isOn = true;
        }



        //========RESOLUCOES========//
        if (PlayerPrefs.HasKey("Resolutions"))
        {
            currentResolucaoId = PlayerPrefs.GetInt("Resolutions");
            Screen.SetResolution((int)supportedResolutions[currentResolucaoId].x, (int)supportedResolutions[currentResolucaoId].y, fullScreenState);
            //Screen.SetResolution(supportedResolutions[currentResolucaoId].width, supportedResolutions[currentResolucaoId].height, fullScreenState);
            resolucaoDropdown.value = currentResolucaoId;
        }
        else
        {
            currentResolucaoId = supportedResolutions.Length - 1;
            Screen.SetResolution((int)supportedResolutions[currentResolucaoId].x, (int)supportedResolutions[currentResolucaoId].y, fullScreenState);
            //Screen.SetResolution(supportedResolutions[currentResolucaoId].width, supportedResolutions[currentResolucaoId].height, fullScreenState);
            PlayerPrefs.SetInt("Resolutions", currentResolucaoId);
            resolucaoDropdown.value = currentResolucaoId;
        }



        //=========QUALIDADES=========//
        if (PlayerPrefs.HasKey("GraphicQuality"))
        {
            graphicQuality = PlayerPrefs.GetInt("GraphicQuality");
            QualitySettings.SetQualityLevel(graphicQuality);
            qualityDropdown.value = graphicQuality;
        }
        else
        {
            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
            graphicQuality = QualitySettings.names.Length - 1;
            PlayerPrefs.SetInt("GraphicQuality", graphicQuality);
            qualityDropdown.value = graphicQuality;
        }

        //=========LANGUAGE=========//
        if (PlayerPrefs.HasKey("Language"))
        {
            currentLanguage = PlayerPrefs.GetInt("Language");
            GameManager.languageId = currentLanguage;
            languageDropdown.value = currentLanguage;
        }
        else
        {
            GameManager.languageId = 0;
            currentLanguage = 0;
            PlayerPrefs.SetInt("Language", currentLanguage);
            languageDropdown.value = currentLanguage;
        }

        continueButton.SetActive(File.Exists(path));
    }

    private void AddResolutions()
    {
        resolucaoDropdown.options.Clear();

        for (int i = 0; i < supportedResolutions.Length; i++)
            resolucaoDropdown.options.Add(new TMP_Dropdown.OptionData() { text = supportedResolutions[i].x + "x" + supportedResolutions[i].y });

        resolucaoDropdown.captionText.text = "Resolutions";
    }


    private void AdjustQualities()
    {
        string[] nomes = QualitySettings.names;
        qualityDropdown.options.Clear();

        for (int i = 0; i < nomes.Length; i++)
            qualityDropdown.options.Add(new TMP_Dropdown.OptionData() { text = nomes[i] });

        qualityDropdown.captionText.text = "Quality";
    }

    private void SetLanguage()
    {
        string[] names;
        if (GameManager.languageId == 0)
            names = languageEn;
        else
            names = languagePt;

        languageDropdown.options.Clear();

        for (int i = 0; i < languageEn.Length; i++)
            languageDropdown.options.Add(new TMP_Dropdown.OptionData() { text = names[i] });

        languageDropdown.captionText.text = "Language";
    }

    public void SavePrefes()
    {
        if (fullScreenToggle.isOn == true)
        {
            windowMode = 0;
            fullScreenState = true;
        }
        else
        {
            windowMode = 1;
            fullScreenState = false;
        }

        PlayerPrefs.SetFloat("SFX_volume", sfxSlider.value);
        PlayerPrefs.SetFloat("Music_volume", musicSlider.value);
        PlayerPrefs.SetInt("GraphicQuality", qualityDropdown.value);
        PlayerPrefs.SetInt("WindowMode", windowMode);
        PlayerPrefs.SetInt("Language", languageDropdown.value);
        PlayerPrefs.SetInt("Resolutions", resolucaoDropdown.value);
        PlayerPrefs.SetFloat("Sensibility", sensibility);
        currentResolucaoId = resolucaoDropdown.value;
        ApplyPrefs();
    }

    private void ApplyPrefs() {

        sfxVolume = PlayerPrefs.GetFloat("SFX_volume");
        musicVolume = PlayerPrefs.GetFloat("Music_volume");
        sensibility = PlayerPrefs.GetFloat("Sensibility");
        //setar volumes;    
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualidadeGrafica"));
        Screen.SetResolution((int)supportedResolutions[currentResolucaoId].x, (int)supportedResolutions[currentResolucaoId].y, fullScreenState);
        //GameManager.languageId = PlayerPrefs.GetInt("Language");
        int i = PlayerPrefs.GetInt("Language");
        GameManager.m_Instance.ChangeLanguage(i);

    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenGraphicsMenu()
    {
        graphicsMenu.SetActive(true);
        audioMenu.SetActive(false);
        interfaceMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OpenAudioMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(true);
        interfaceMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OpenInterfaceMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);
        interfaceMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }    
    
    public void OpenControlsMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);
        interfaceMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }



    public void BackToMainMenu()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);
        settingsMenu.SetActive(false);
        interfaceMenu.SetActive(false);
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TryNewGame()
    {
        if (File.Exists(path))
        {
            newGameWarning?.SetActive(true);
        }
        else
        {
            PlayGame();
        }
    }

    public void NewGame()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LoadScene");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LoadScene");
        //SceneManager.LoadSceneAsync("MainScene");
    }

    public void SetSensibility(float value)
    {
        sensibility += value;

        if (sensibility > 8)
        {
            sensibility = 8;
        }
        else if (sensibility < 1)
        {
            sensibility = 1;
        }

        sensibilityText.text = sensibility.ToString("F1");
    }

    void OnGUI()
    {
        if(!deletePlayerPrefs)
        return;

        if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
            PlayerPrefs.DeleteAll();
    }
}
