using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public Animator anim;
    public string m_animation;

    // Start is called before the first frame update
    void Start()
    {
        anim.Play(m_animation,0,0);    
    }
}
