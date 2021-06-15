using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrainingSettings : MonoBehaviour
{
    public Role role;
    public Dropdown AISelection;
    public InputField inputPrefab;
    public Dropdown dropdown;

    Vector3 currentPos;
    Dropdown championSelection;
    Queue<InputField> primitiveInputs = new Queue<InputField>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Fill();
    }

    private void Fill()
    {
        AISelection.ClearOptions();

        foreach (AIController ai in AIOptions.Options)
        {
            AISelection.options.Add(new Dropdown.OptionData(ai.name));
        }

        AISelection.RefreshShownValue();
    }

    public void OnSelectedChange()
    {
        AIController selected = AIOptions.Options[AISelection.value];

        DestroyAllInputs();
        currentPos = AISelection.transform.position;        

        Type trainer = selected.GetType();

        if (trainer.IsSubclassOf(typeof(AITrainer)))
        {
            FillChampionList(trainer);
        }


        var variables = trainer.GetFields().Where(field => field.IsPublic);

        foreach (var variable in variables)
        {
            if (variable.FieldType.IsPrimitive)
            {
                var input = Instantiate(inputPrefab, currentPos, Quaternion.identity, this.transform);
                float height = this.GetComponent<RectTransform>().rect.height;
                input.transform.localPosition  -= new Vector3(0, height, 0);
                input.placeholder.gameObject.GetComponent<Text>().text = variable.Name;
                currentPos = input.transform.position;
                primitiveInputs.Enqueue(input);                
            }
                
        }
    }

    public AIController GetSelected()
    {
        return AIOptions.Options[AISelection.value];
    }

    private void FillChampionList(Type trainer)
    {
        string directoryPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Trained", trainer.Name);

        if (!Directory.Exists(directoryPath))
            return;

        var input = Instantiate(dropdown, currentPos, Quaternion.identity, this.transform);
        float height = this.GetComponent<RectTransform>().rect.height;
        input.transform.localPosition -= new Vector3(0, height, 0);
        currentPos = input.transform.position;
        championSelection = input;

        string[] files = Directory.GetFiles(directoryPath);

        championSelection.ClearOptions();
        championSelection.options.Add(new Dropdown.OptionData("None"));

        foreach (var file in files)
            championSelection.options.Add(new Dropdown.OptionData(Path.GetFileName(file)));
    }

    private void DestroyAllInputs()
    {
        foreach (InputField inputField in primitiveInputs)
            Destroy(inputField.gameObject);

        primitiveInputs.Clear();

        DestroyChampSelection();
        
    }

    private void DestroyChampSelection()
    {
        Destroy(championSelection);
        championSelection = null;
    }  
    
    public void InitializeVariables(AIController controller)
    {
        var variables = controller.GetType().GetFields().Where(field => field.IsPublic);

        foreach (var variable in variables)
        {
            if (variable.FieldType.IsPrimitive)
            {
                variable.SetValue(controller, primitiveInputs.Dequeue());
            }

        }
    }

}
    
