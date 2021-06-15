using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonLoader : MonoBehaviour
{
    [SerializeField]
    UnitButton buttonPrefab;

    private void Start()
    {
        Vector3 startPos = transform.position;
        for (int i = 0; i < UnitFinder.UnitStats.Count; i++)
        {
            UnitButton button = Instantiate(buttonPrefab, startPos, Quaternion.identity, this.transform);

            float height = buttonPrefab.GetComponent<RectTransform>().rect.height;

            button.transform.localPosition -= new Vector3(0, height + 2, 0);
            button.Set(i, UnitFinder.UnitStats[i].UnitType.Name, UnitFinder.UnitStats[i].Price);

            startPos = button.transform.position;
        }
    }
}
