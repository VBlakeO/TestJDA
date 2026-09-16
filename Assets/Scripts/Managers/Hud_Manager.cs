using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Hud_Manager : MonoBehaviour
{
    public static Hud_Manager m_Instance;

    [SerializeField] private TextMeshProUGUI m_WallMoneyText = null;
    [SerializeField] private TextMeshProUGUI m_MoneyText = null;
    [SerializeField] private Image m_AimThrowing = null;
    [SerializeField] private Image m_AimCircule = null;
    [SerializeField] private Image m_AimBase = null;
    [Space]

    [SerializeField] private GameObject m_Catch = null;
    [SerializeField] private GameObject m_Interact = null;
    [SerializeField] private GameObject m_Rotate = null;
    [SerializeField] private GameObject m_Throw = null;


    [SerializeField] private GameObject droneUI = null;
    

    private void Awake() => m_Instance = this;

    private void Start() 
    {
        UpdateMoneyText();
        SavableGameData.OnDepositMoney += UpdateMoneyText;
        SavableGameData.OnWithdrawMoney += UpdateMoneyText;
    }

    public void LockCursor(bool state)
    {
        if(state)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        Cursor.visible = !state;
    }


    public void SetDroneUIState(bool state)
    {
        droneUI?.SetActive(state);
    }

    public void UpdateMoneyText()
    {
        int money = (int)SavableGameData.CurrentMoney;
        m_MoneyText.text = "$" + money.ToString();
        m_WallMoneyText.text = "$" + money.ToString();
    }

    public void ActiveAim(bool active) => m_AimBase.gameObject.SetActive(active);

    public void ActiveAimCircule(bool active) => m_AimCircule.gameObject.SetActive(active);

    public void ActiveCatch(bool active) => m_Catch.SetActive(active);

    public void ActiveInteractiveInfo(bool active) => m_Interact.SetActive(active);

    public void ActiveRotateInfo(bool active)
    {
        m_Rotate.SetActive(active);
        m_Throw.SetActive(active);
    }

    public void ActiveThrowInfo(bool active)
    {
    }

    public void ThrowingStrength(float value) => m_AimThrowing.fillAmount = value;
}
