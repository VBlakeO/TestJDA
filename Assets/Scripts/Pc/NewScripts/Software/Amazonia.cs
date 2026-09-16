using System;
using System.Collections.Generic;
using System.IO;
//using Newtonsoft.Json;
using UnityEngine;

public class Amazonia : BasePage, IPersistent
{
    public static Amazonia Instance;

    public Transform SpawnPoint = null;
    public GameObject boxPrefab = null;
    public GameObject[] itemsPrefab;
    [Space]

    public TranslateTool translateTool = null;
    [SerializeField] Data _data = new Data();
    public Data data => _data;


    public Dictionary<int, SavableObjectData> amazoniaItens => data.amazoniaItens;
    string _persistentPath = "/Amazonia";


    private void Awake()
    {
        Instance = this;
        ((IPersistent)this).Subscribe();
    }
    
    protected override void Start()
    {
        translateTool.ChangeLanguage(GameManager.languageId);

    }

    public void BuyItem(int id) // 
    {
        AmazoniaBox newBox = Instantiate(boxPrefab, SpawnPoint.position, SpawnPoint.rotation).GetComponent<AmazoniaBox>();
        newBox.prefab = itemsPrefab[id];
        newBox.item = id;
    }

    #region SaveInterface
    void IPersistent.Subscribe()
    {
        Persistent.Instance.onSave += ((IPersistent)this).SaveAsJson;
        Persistent.Instance.onLoad += ((IPersistent)this).LoadFromJson;
    }

    void IPersistent.Unsubscribe()
    {
        Persistent.Instance.onSave -= ((IPersistent)this).SaveAsJson;
        Persistent.Instance.onLoad -= ((IPersistent)this).LoadFromJson;
    }

    public bool HasJsonSave(string persistentDataPath)
    {
        return File.Exists(persistentDataPath + _persistentPath);
    }

    public void LoadFromJson(string persistentDataPath)
    {
        if (!((IPersistent)this).HasJsonSave(persistentDataPath))
        {
            ((IPersistent)this).SaveAsJson(persistentDataPath);
            return;
        }

       // _data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(persistentDataPath + _persistentPath));
        
        SpawnSavedItens();
    }

    public void SaveAsJson(string persistentDataPath)
    {
        //File.WriteAllText(persistentDataPath + _persistentPath, JsonConvert.SerializeObject(_data, Formatting.Indented));
    }
    #endregion

    private void SpawnSavedItens()
    {
        foreach (var item in amazoniaItens)
        {
            Vector3 pos = new(item.Value.Position[0], item.Value.Position[1], item.Value.Position[2]);
            SavableObject obj = Instantiate(itemsPrefab[item.Value.Item], pos, Quaternion.identity).GetComponent<SavableObject>();
            obj.Load(item.Value);
        }
    }

    [Serializable]
    public class Data
    {
        public Dictionary<int, SavableObjectData> amazoniaItens = new Dictionary<int, SavableObjectData>();
    }
}
