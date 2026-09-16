
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float time = 5;
    public string axis = "x";
    public Transform load;
	
    void OnEnable()
    {
        iTween.RotateBy(load.gameObject, iTween.Hash(axis, -1, "time", time, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
    }
}
