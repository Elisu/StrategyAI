using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsoleController : MonoBehaviour
{
    public GameObject manager;

    public void ToggleChecked(bool toggle)
    {
        manager.SetActive(toggle);
    }
}
