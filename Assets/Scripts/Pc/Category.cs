using UnityEngine;

[CreateAssetMenu(fileName = "Category", menuName = "Scriptable/Category")]
public class Category : ScriptableObject
{
    public Sprite m_Icon;
    public string[] m_Namme = null;
    public float  m_Difficulty;
    public float  m_Cost;
}
