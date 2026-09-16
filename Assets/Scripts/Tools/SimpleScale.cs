using System.Collections;
using UnityEngine;

public class SimpleScale : MonoBehaviour
{
    public Vector3 m_InitialScale;
    public Vector3 m_FinalScale = Vector3.one;
    [Space]
    public float time = 0.8f;

    void OnEnable()
    {
        transform.localScale = m_InitialScale;
        iTween.ScaleTo(gameObject, iTween.Hash("scale", m_FinalScale, "time", time, "easetype", "linear"));
    }

    public void ScaleObject()
    {
        transform.localScale = m_InitialScale;
        iTween.ScaleTo(gameObject, iTween.Hash("scale", m_FinalScale, "time", time, "easetype", "linear"));
    }

    public void ScaleObjectDelay()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", m_InitialScale, "time", time, "easetype", "linear"));
        Invoke("RevertScale", time);
    }

    void RevertScale()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", m_FinalScale, "time", time, "easetype", "linear"));
    }

    private IEnumerator ScaleDelay()
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        iTween.ScaleTo(gameObject, iTween.Hash("scale", m_InitialScale, "time", time, "easetype", "linear"));

        yield return wfs;
        iTween.ScaleTo(gameObject, iTween.Hash("scale", m_FinalScale, "time", time, "easetype", "linear"));
    }
}
