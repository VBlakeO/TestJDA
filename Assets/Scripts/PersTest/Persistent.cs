using System;
using UnityEngine;

public class Persistent : MonoBehaviour
{
    public static Persistent Instance;
    public string persistentDataPath { get; private set; }

    public Action OnPreSave; //string path
    public bool save = true; 

    public Action<string> onSave; //string path
    public Action<string> onLoad; //string path

    private void Awake()
    {
        Instance = this;
        persistentDataPath = Application.persistentDataPath;
    }

    void Start()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        if (save)
            Save();
    }


    public void Save()
    {
        OnPreSave?.Invoke();
        onSave?.Invoke(persistentDataPath);
    }

    public void Load()
    {
        onLoad?.Invoke(persistentDataPath);
    }
}