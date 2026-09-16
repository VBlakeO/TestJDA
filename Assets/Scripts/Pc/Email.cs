using UnityEngine;

[CreateAssetMenu(fileName = "Email", menuName = "Scriptable/Email")]
public class Email : ScriptableObject
{
    public string[] subject = null;
    [Space]
    [TextArea(5, 30)] 
    public string[] email = null;
}
