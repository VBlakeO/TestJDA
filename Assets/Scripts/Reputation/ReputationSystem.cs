using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReputationSystem : MonoBehaviour
{
    public static ReputationSystem m_Instance;

    public float ReputationRequired = 100f;
    
    [SerializeField] private Image reputationBar = null;
    [SerializeField] private float percentage = 0f;
    [SerializeField] private int currentTier = 0;

    public ReputationInfo reputationInfo = null;

    public UnityAction<int> OnTierChange;
    public UnityAction OnMaxReputationIsReached;
    public UnityAction <float>OnReputationGhange;

    private void Awake()
    {
        m_Instance = this;   
        UpdateReputationBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
            ApplyReputation(10);

        if (Input.GetKeyDown(KeyCode.F6))
            RemoveReputation();
    }

    public void ApplyReputation(float estimatedReputation)
    {
        float value = estimatedReputation + (reputationInfo.reputationPerEmployee * EmployeeManager.Instance.employees.Count);
        SavableGameData.reputation = Mathf.Clamp(SavableGameData.reputation += value, 0f, ReputationRequired);
        UpdateReputationBar();

        if (RequiredReputationAchieved())
            OnMaxReputationIsReached?.Invoke();
    }

    public void RemoveReputation()
    {
        SavableGameData.reputation = Mathf.Clamp(SavableGameData.reputation -= reputationInfo.reputationReduction, 0f, ReputationRequired);
        UpdateReputationBar();
    }

    public void RemoveReputation(float reduction)
    {
        SavableGameData.reputation = Mathf.Clamp(SavableGameData.reputation -= reduction, 0f, ReputationRequired);
        UpdateReputationBar();
    }

    private void UpdateReputationBar()
    {   
        percentage = SavableGameData.reputation / ReputationRequired * 100f;
        GetReputationTier(percentage);

        OnReputationGhange?.Invoke(percentage);

        reputationBar.fillAmount = SavableGameData.reputation / ReputationRequired; 
    }
    

    public int GetReputationTier(float percentage)
    {
        if (percentage >= 30f && percentage < 60f)
        {
            if (currentTier != 1)
            { 
                currentTier = 1; 
                UpdateTier();
            }
        }
        else if (percentage >= 60f && percentage < 90f)
        {
            if (currentTier != 2)
            {
                currentTier = 2;
                UpdateTier();
            }
        }
        else if (percentage >= 90)
        {
            if (currentTier != 3)
            {
                currentTier = 3;
                UpdateTier();
            }
        }

        return currentTier;
    }

    private void UpdateTier()
    {
        OnTierChange?.Invoke(currentTier);
    }

    public bool RequiredReputationAchieved()
    {
        return SavableGameData.reputation >= ReputationRequired;
    }
}
