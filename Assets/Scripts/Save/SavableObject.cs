using UnityEngine;

public class SavableObject : MonoBehaviour
{
    public int id = 0;
    public int item = 0;

    public int GetNewId()
    {
        int t = Random.Range(1, 100);

        while (Amazonia.Instance.amazoniaItens.ContainsKey(t))
            t = Random.Range(1, 100);

        return t;
    }

    private void Start() 
    {
        if (id == 0)
            id = GetNewId();

        Persistent.Instance.OnPreSave += PreSave;
    }

    private void PreSave()
    {
        if (Amazonia.Instance.amazoniaItens.ContainsKey(id))
            Amazonia.Instance.amazoniaItens.Remove(id);

        Vector3 pos = transform.position;
        SavableObjectData savableObjectData = new(id, item, new float[3] { pos.x, pos.y, pos.z});

        Amazonia.Instance.amazoniaItens.Add(id, savableObjectData);
    }

    public void Load(SavableObjectData data)
    {
        id = data.Id;
        item = data.Item;
    }

    public void RemoveItem()
    {
        Persistent.Instance.OnPreSave -= PreSave;

        if (Amazonia.Instance.amazoniaItens.ContainsKey(id))
            Amazonia.Instance.amazoniaItens.Remove(id);
    }

    private void OnDestroy()
    {
        Persistent.Instance.OnPreSave -= PreSave;
    }
}

[System.Serializable]
public struct SavableObjectData
{
    public int Id;
    public int Item;
    public float[] Position;

    public SavableObjectData(int _id, int _item, float[] _position)
    {
        Id = _id;
        Item = _item;
        Position = _position;
    }
}
