using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

internal class Loop : MonoBehaviour
{
    [SerializeField]
    protected GameObject mapObject;

    protected List<List<Transform>> LoadMap()
    {
        List<List<Transform>> mapPrefab = new List<List<Transform>>();

        foreach (Transform row in mapObject.transform)
        {
            List<Transform> fields = new List<Transform>();

            foreach (Transform field in row.gameObject.transform)
                fields.Add(field);

            mapPrefab.Add(fields);
        }

        return mapPrefab;
    }

}
