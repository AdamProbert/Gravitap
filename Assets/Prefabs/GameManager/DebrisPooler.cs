﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisPooler : MonoBehaviour
{
    public static DebrisPooler instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public bool playerSaysYehaw = true;

    void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        Transform debrisHolder = transform.Find("DebrisBits");
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            obj.transform.parent = debrisHolder;
            pooledObjects.Add(obj);
        }

    }

    public GameObject GetPooledObject()
    {
        if (playerSaysYehaw)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
        }
        Debug.Log("Debris pooler: Aint got no debris left");
        return null;
    }
}
