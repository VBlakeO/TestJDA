using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPooling : MonoBehaviour
{
    [SerializeField] private List<GameObject> poolingObjct;
    private int activateObjct = 0;

    public GameObject ActivateObject()
    {
        if (activateObjct >= poolingObjct.Count)
            activateObjct = 0;

        poolingObjct[activateObjct].gameObject.SetActive(true);

        activateObjct++;

        return poolingObjct[activateObjct].gameObject;
    }

    
}
