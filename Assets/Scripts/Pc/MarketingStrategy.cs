using UnityEngine;

[CreateAssetMenu(fileName = "MarketingStrategy", menuName = "Scriptable/MarketingStrategy")]
public class MarketingStrategy : ScriptableObject 
{
    public Sprite image = null;
    public string[] namme = null;
    [Space]

    [TextArea(5, 20)] 
    public string[] description = null;
    [Space]

    public float cost = 0f;
    public float disclosure = 0f;
}
