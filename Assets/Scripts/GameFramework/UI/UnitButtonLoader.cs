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
        for (int i = 0; i < UnitFinder.unitStats.Count; i++)
        {
            if (buttonPrefab.TryGetComponent<RectTransform>(out RectTransform rect))
                startPos.y -= rect.rect.height + 2;
            UnitButton button = Instantiate(buttonPrefab, startPos, Quaternion.identity, this.transform);
            button.Set(i, UnitFinder.unitStats[i].UnitType.Name, UnitFinder.unitStats[i].Price);
        }
    }
}
