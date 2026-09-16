using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string scene = "";
    public float time = 3f;
    public bool playAwake = false;

    private void Start()
    {
        if (playAwake)
            StartCoroutine(LoadScene(scene));
    }

    public IEnumerator LoadScene(string _scene)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync(_scene);
    }
}
