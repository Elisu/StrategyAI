using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMasterScript : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject mapObject;

    void Start()
    {
        List<List<Transform>> mapPrefab = new List<List<Transform>>();

        foreach (Transform row in mapObject.transform)
        {
            List<Transform> fields = new List<Transform>();

            foreach (Transform field in row.gameObject.transform)
                fields.Add(field);

            mapPrefab.Add(fields);
        }

        MasterScript.map = new IObjectMap(height, width, mapPrefab);
    }
}
