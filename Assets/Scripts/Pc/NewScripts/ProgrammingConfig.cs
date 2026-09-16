using UnityEngine;

[CreateAssetMenu(fileName = "ProgrammingConfig", menuName = "Scriptable/Programming")]
public class ProgrammingConfig : ScriptableObject
{
    [Header("Player")]
    [Min(0f)] public float leaderMultiplier = 1.5f;
    [Min(1)] public int maxKeystrokesPerSecond = 10;
    [Space]

    [Header("Editor Display")]
    [Min(1)] public int charsPerKeystroke = 10;
    [Min(1)] public int maxVisibleLines = 30;
    [Min(1)] public int maxVisibleCharacters = 2000;
}