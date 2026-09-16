using UnityEngine.UI;
using UnityEngine;

public class DevOp_ButtonsManager : MonoBehaviour
{
    public NewDevOp devlopment = null;
    [Space]

    [Header("Side Bar")]
    public Button NewGameButton = null;
    public Button MyGamesButton = null;

    [Header("Small Side Bar")]
    public Button MyGamesSmallButton = null;

    [Header("Setting Tab")]
    public Button LeftArrowButton = null;
    public Button RightArrowButton = null;
    [Space]
    public Button PreviousButton = null;
    public Button NextButton = null;

    [Header("Creation Tab")]
    public Button ProjectCompletedButton = null;

    [Header("SaleTab")]
    public Button SellGameButton = null;

    [Header("VFX")]
    public SimpleScale settingIcon = null;
    public SimpleScale settingNammeText = null;

    private void Start()
    {
        NewGameButton.onClick.AddListener(NewGameButtonEvent);
        MyGamesButton.onClick.AddListener(MyGamesButtonEvent);
       
        MyGamesSmallButton.onClick.AddListener(MyGamesSmallButtonEvent);

        LeftArrowButton.onClick.AddListener(LeftArrowButtonEvent);
        RightArrowButton.onClick.AddListener(RightArrowButtonEvent);

        PreviousButton.onClick.AddListener(PreviousButtonEvent);
        NextButton.onClick.AddListener(NextButtonEvent);

        ProjectCompletedButton.onClick.AddListener(ProjectCompletedEvent);
        SellGameButton.onClick.AddListener(SellGameEvent);
    }

    private void OnEnable() 
    {
        ProjectCompletedButton.interactable = true;
        NewGameButton.interactable = true;
        MyGamesButton.interactable = true;
    }

    //==============Side Bar==============//
    public void NewGameButtonEvent()
    {
        devlopment.OpenTabs(0);
        devlopment.EnableSideBar(false);

        devlopment.navigationTabsButtons.SetActive(true);
        devlopment.UpdateDevSetting();

        NewGameButton.interactable = false;
        MyGamesButton.interactable = true;
    }

    public void MyGamesButtonEvent()
    {
        //devlopment.ResetAll();
        devlopment.OpenTabs(2);
        devlopment.CallOpenSavedGames();

        devlopment.navigationTabsButtons.SetActive(false);

        NewGameButton.interactable = true;
        MyGamesButton.interactable = false;
    }


    //==============Small Side Bar==============//
    public void MyGamesSmallButtonEvent()
    {
        //devlopment.ResetAll();
        devlopment.OpenTabs(2);
        devlopment.CallOpenSavedGames();

        devlopment.EnableSideBar(true);
        devlopment.navigationTabsButtons.SetActive(false);

        NewGameButton.interactable = true;
        MyGamesButton.interactable = false;
    }


    //==============SettingTab==============//
    public void LeftArrowButtonEvent()
    {
        devlopment.ChangeCategory(false);

        settingIcon.ScaleObjectDelay();
        settingNammeText.ScaleObjectDelay();
    }

    public void RightArrowButtonEvent()
    {
        devlopment.ChangeCategory(true);

        settingIcon.ScaleObjectDelay();
        settingNammeText.ScaleObjectDelay();
    }

    public void PreviousButtonEvent()
    {
        devlopment.ChangePage(false);
      
        settingIcon.ScaleObjectDelay();
        settingNammeText.ScaleObjectDelay();
    }

    public void NextButtonEvent()
    {
        devlopment.ChangePage(true);
        
        settingIcon.ScaleObjectDelay();
        settingNammeText.ScaleObjectDelay();
    }

    public void ProjectCompletedEvent()
    {
        ProjectCompletedButton.interactable = !devlopment.CompleteProjectPlanning();
        ProjectCompletedButton.GetComponent<SimpleScale>().ScaleObjectDelay();
    }


    //==============SaleTab==============//
    public void SellGameEvent()
    {
        devlopment.SellGame();
    }

}
