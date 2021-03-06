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
        MasterScript.map = new Map<IObject>(width, height, mapObject);

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                MasterScript.map[i, j] = new Grass();
            }
    }
}
