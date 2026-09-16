using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> poolingObjctList;
    [SerializeField] private GameObject objctPrefab = null;
    [SerializeField] private RectTransform rectTransform = null;

    private int activateObjct = 0;

    private void Start() 
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(objctPrefab, rectTransform);
            poolingObjctList.Add(obj);
        }    
    }

    public GameObject ActivateObject()
    {
        if (activateObjct >= poolingObjctList.Count)
            activateObjct = 0;

        if (poolingObjctList[activateObjct].activeInHierarchy)
            AddNewObject();
        else
            poolingObjctList[activateObjct].gameObject.SetActive(true);

        activateObjct++;

        return poolingObjctList[activateObjct].gameObject;
    }


    public void AddNewObject()
    {

    }
}

















/*
        [SerializeField] private Transform[] poolingObjct;
    private int activateObjct = 0;

    public GameObject ActivateObject(Transform origin)
    {
        if (activateObjct < poolingObjct.Length)
        {
            poolingObjct[activateObjct].SetPositionAndRotation(origin.position, Quaternion.identity);
            poolingObjct[activateObjct].gameObject.SetActive(true);

            activateObjct++;
            return poolingObjct[activateObjct].gameObject;
        }
        else
        {
            activateObjct = 0;
            poolingObjct[activateObjct].SetPositionAndRotation(origin.position, Quaternion.identity);
            poolingObjct[activateObjct].gameObject.SetActive(true);

            activateObjct++;
            return poolingObjct[activateObjct].gameObject;
        }
    }

*/