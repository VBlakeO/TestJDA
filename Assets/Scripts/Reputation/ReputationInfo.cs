using UnityEngine;

[CreateAssetMenu(fileName = "Reputation", menuName = "Scriptable/Reputation")]
public class ReputationInfo : ScriptableObject
{
    public float reputationPerGame = 0f;
    public float reputationPerEmployee = 0f;
    [Space]

    public float reputationReduction = 0f;
}
