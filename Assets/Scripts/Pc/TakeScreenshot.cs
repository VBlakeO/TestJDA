using System.Collections;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
    [SerializeField] private Camera m_camera = null;
    private bool studioMode = false;

    private int GetResolution() => (int)(Screen.width * (26.67f / 100f));

    private void Start()
    {
        m_camera = GetComponent<Camera>();
        m_camera.enabled = false;
    }

    public void TakeScreenshoot(bool _studioMode)
    {
        studioMode = _studioMode;
        StartCoroutine(Screenshoot());
    }

    private IEnumerator Screenshoot()
    {
        m_camera.enabled = true;

        yield return new WaitForEndOfFrame();

        int res = GetResolution();
        Texture2D screenshotTexture = new Texture2D(res, res, TextureFormat.RGBA32, false);
        Rect rect = new Rect(0, 0, res, res);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArrey = screenshotTexture.EncodeToPNG();

        if (studioMode)
        {
            SavableGameData.studioByte = byteArrey;
            MyStudio.m_Instance.LoadSprites();
        }
        else
        {
            SavableGameData.gameByte.Add(byteArrey);
            NewDevOp.Instance.devPublishingTab.LoadSprites();
        }

        m_camera.enabled = false;
    }
}