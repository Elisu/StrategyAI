using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject console;
    public void OnEndClick()
    {
        Application.Quit();
    }

    public void OnMenuClick()
    {
        var controllers = FindObjectsOfType<AIController>();

        if (console != null)
            Destroy(console.gameObject);

        foreach (var controller in controllers)
            Destroy(controller.gameObject);

        SceneManager.LoadScene(0);
    }
}
