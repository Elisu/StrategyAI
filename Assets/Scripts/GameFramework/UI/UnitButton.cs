using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    int unitIndex;
    HumanPlayerController controller;

    public void Set(int index, string name, int price)
    {
        Button button = GetComponent<Button>();
        unitIndex = index;
        button.onClick.AddListener(Clicked);
        button.GetComponentInChildren<Text>().text = name + " " + price.ToString();
    }

    private void Clicked()
    {
        if (controller == null)
            controller = FindObjectOfType<HumanPlayerController>();

        controller.AddUnit(unitIndex);
    }
}
