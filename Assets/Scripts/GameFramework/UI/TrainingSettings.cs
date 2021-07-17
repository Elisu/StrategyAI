using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

internal class TrainingSettings : MonoBehaviour
{
    public Role role;
    public Dropdown AISelection;
    public InputField inputPrefab;
    public InputField floatInputPrefab;
    public Dropdown dropdown;
    public LoadedAI loadedAI;

    public Toggle myToggle;
    public Toggle enemyToggle;

    Vector3 currentPos;
    Dropdown championSelection;
    Queue<InputField> primitiveInputs = new Queue<InputField>();

    // Start is called before the first frame update
    void Start()
    {
        Fill();
        OnSelectedChange();
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

    public void OnChampionSelection()
    {
        if (championSelection.value != 0)
            DestroyParamInput();
        else
            OnSelectedChange();

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
                InputField input = null;

                if (variable.FieldType == typeof(int))
                    input = Instantiate(inputPrefab, currentPos, Quaternion.identity, this.transform);
                else if (variable.FieldType == typeof(float))
                    input = Instantiate(floatInputPrefab, currentPos, Quaternion.identity, this.transform);

                float height = this.GetComponent<RectTransform>().rect.height;
                input.transform.localPosition  -= new Vector3(0, height, 0);
                input.placeholder.gameObject.GetComponent<Text>().text = variable.Name;
                currentPos = input.transform.position;
                primitiveInputs.Enqueue(input);                
            }
                
        }
    }

    public void OnSelectedChangeGame()
    {
        DestroyChampSelection();
        currentPos = AISelection.transform.position;

        AIController selected = AIOptions.Options[AISelection.value];

        Type trainer = selected.GetType();

        if (trainer.IsSubclassOf(typeof(AITrainer)))
        {
            FillChampionList(trainer);
        }
    }

    public void OnMyToggleChange(bool check)
    {
        if (check)
            enemyToggle.isOn = false;
            
    }

    public AIController GetSelected()
    {
        return AIOptions.Options[AISelection.value];
    }

    public string GetChampion()
    {
        
        if (championSelection == null || championSelection.value == 0)
            return null;
        else
            return championSelection.options[championSelection.value].text;
    }

    private void FillChampionList(Type trainer)
    {
        string directoryPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Trained", trainer.Name);

        if (!Directory.Exists(directoryPath))
            return;

        championSelection = Instantiate(dropdown, currentPos, Quaternion.identity, this.transform);
        float height = this.GetComponent<RectTransform>().rect.height;
        championSelection.transform.localPosition -= new Vector3(0, height, 0);
        currentPos = championSelection.transform.position;

        string[] files = Directory.GetFiles(directoryPath);

        championSelection.ClearOptions();
        championSelection.options.Add(new Dropdown.OptionData("None"));
        championSelection.onValueChanged.AddListener(delegate { OnChampionSelection(); }); ;

        foreach (var file in files)
            championSelection.options.Add(new Dropdown.OptionData(Path.GetFileName(file)));
    }

    private void DestroyAllInputs()
    {
        DestroyParamInput();
        DestroyChampSelection();
        
    }

    private void DestroyParamInput()
    {
        foreach (InputField inputField in primitiveInputs)
            Destroy(inputField.gameObject);

        primitiveInputs.Clear();
    }

    private void OnDisable()
    {
        DestroyAllInputs();
    }

    private void DestroyChampSelection()
    {
        if (championSelection != null)
        {
            Destroy(championSelection.gameObject);
            championSelection = null;
        }            
    }  
    
    public void InitializeVariables()
    {
        var selected = GetSelected();

        int champion = 0;

        if (championSelection != null)
            champion = championSelection.value;

        string championFile = null;

        if (champion != 0)
            championFile = championSelection.options[champion].text;

        selected = TrainingLoop.InstantiateController(championFile, selected, loadedAI);
        selected.tag = role.ToString();
        DontDestroyOnLoad(selected);

        var variables = selected.GetType().GetFields().Where(field => field.IsPublic);

        foreach (var variable in variables)
        {
            if (variable.FieldType.IsPrimitive)
            {
                try
                {
                    var input = primitiveInputs.Peek();

                    if (variable.FieldType == typeof(float) && float.TryParse(input.text, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
                        variable.SetValue(selected, result);
                    else if (variable.FieldType == typeof(int) && int.TryParse(input.text, out int result2))
                        variable.SetValue(selected, result2);
                    else
                    {
                        Destroy(selected.gameObject);
                        return;
                    }

                    primitiveInputs.Dequeue();
                }
                catch (Exception _e)
                {
                    Debug.LogError("Something went wrong while loading input");
                }
            }

        }
    }

}
    
