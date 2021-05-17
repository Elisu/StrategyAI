using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrainingDropDownFill : MonoBehaviour
{
    public Dropdown AISelection;
    public Transform CanvasParent;    

    public InputField inputPrefab;

    Vector3 currentPos;
    List<InputField> primitiveInputs = new List<InputField>();

    // Start is called before the first frame update
    void Start()
    {
        Fill();
    }

    private void Fill()
    {
        AISelection.ClearOptions();

        foreach (AITrainer ai in AIOptions.Options)
        {
            AISelection.options.Add(new Dropdown.OptionData(ai.name));
        }

        AISelection.RefreshShownValue();
    }

    public void OnSelectedChange()
    {
        AITrainer selected = AIOptions.Options[AISelection.value];

        DestroyAllInputs();
        currentPos = AISelection.transform.position;        

        Type trainer = selected.GetType();
        var variables = trainer.GetFields().Where(field => field.IsPublic);

        foreach (var variable in variables)
        {
            if (variable.FieldType.IsPrimitive)
            {
                if (inputPrefab.TryGetComponent<RectTransform>(out RectTransform rect))
                    currentPos.y -= rect.rect.height + 2;
                primitiveInputs.Add(Instantiate(inputPrefab, currentPos, Quaternion.identity, CanvasParent));                
            }
                
        }
    }

    public AITrainer GetSelected()
    {
        return AIOptions.Options[AISelection.value];
    }

    private void DestroyAllInputs()
    {
        foreach (InputField inputField in primitiveInputs)
            Destroy(inputField.gameObject);
        
        primitiveInputs.Clear();
    }

}
    
