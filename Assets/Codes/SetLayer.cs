using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour
{
    public void SetLayerId(int layerId)
    {
        this.gameObject.layer = layerId;
    }

    public void SetLayerIdChildren(int layerId, GameObject obj)
    {
        GameObject[] children = obj.GetComponentsInChildren<GameObject>();
        foreach(GameObject child in children)
        {
            child.layer = layerId;
        }
    }
}
