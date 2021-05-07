using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SettingMasterScript : MonoBehaviour
{
    public bool trainingMode;

    public GameObject mapObject;

    void Awake()
    {
        MasterScript.IsTrainingMode = trainingMode;
        LoadMap();
    }

    private void LoadMap()
    {
        List<List<Transform>> mapPrefab = new List<List<Transform>>();

        foreach (Transform row in mapObject.transform)
        {
            List<Transform> fields = new List<Transform>();

            foreach (Transform field in row.gameObject.transform)
                fields.Add(field);

            mapPrefab.Add(fields);
        }

        //MasterScript.map = new IObjectMap(mapPrefab.Count, mapPrefab[0].Count, mapPrefab);
    }    
}
