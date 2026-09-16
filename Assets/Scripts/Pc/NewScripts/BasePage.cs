using UnityEngine.UI;
using UnityEngine;

public class BasePage : MonoBehaviour
{
    public GameObject SoftwareScreen;
    public Button exitButton;
    public Image highLightIcon = null;

    [HideInInspector]
    public PC_Manager pc_Manager = null;

    protected virtual void Start()
    {
        pc_Manager.OnClosePages += CloseSoftware;
        pc_Manager = PC_Manager.Instance;
        
        if(exitButton) 
            exitButton.onClick.AddListener(CloseSoftware);
    }

    public virtual void OpenSoftware()
    {
        OpenEffect();
        highLightIcon.enabled = true;
    }
    
    public virtual void CloseSoftware()
    {
        SoftwareScreen.SetActive(false);
        highLightIcon.enabled = false;
    }

    private void OpenEffect()
    {
        SoftwareScreen.SetActive(true);
        SoftwareScreen.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        iTween.ScaleTo(SoftwareScreen, iTween.Hash("scale", Vector3.one, "time", 0.2f, "easetype", "linear"));
    }


}
