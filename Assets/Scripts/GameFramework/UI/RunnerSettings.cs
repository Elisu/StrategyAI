using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerSettings : MonoBehaviour
{
    public Dropdown RunnerSelection;
    public InputField inputPrefab;
    public InputField floatInputPrefab;

    public GameObject attacker;
    public GameObject defender;


    Vector3 currentPos;
    Queue<InputField> primitiveInputs = new Queue<InputField>();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Fill();
        OnSelectedChange();
    }

    private void Fill()
    {
        RunnerSelection.ClearOptions();

        foreach (TrainingRunner runner in RunnerOptions.Options)
        {
            RunnerSelection.options.Add(new Dropdown.OptionData(runner.name));
        }

        RunnerSelection.RefreshShownValue();
    }

    public void OnSelectedChange()
    {
        TrainingRunner selected = RunnerOptions.Options[RunnerSelection.value];

        DestroyAllInputs();
        currentPos = RunnerSelection.transform.position;


        Type runner = selected.GetType();
        var variables = runner.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(field => Attribute.IsDefined(field, typeof(ShowInMenuAttribute)));

        int controllerCount = 0;

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
                input.transform.localPosition -= new Vector3(0, height, 0);
                input.placeholder.gameObject.GetComponent<Text>().text = variable.Name;
                currentPos = input.transform.position;
                primitiveInputs.Enqueue(input);
            }

            if (variable.FieldType == typeof(AIController))
            {
                controllerCount++;
            }

        }

        if (controllerCount == 2)
        {
            attacker.SetActive(true);
            defender.SetActive(true);
        }

    }

    private void DestroyAllInputs()
    {
        foreach (InputField inputField in primitiveInputs)
            Destroy(inputField.gameObject);

        primitiveInputs.Clear();

        attacker.SetActive(false);
        defender.SetActive(false);
    }

    public void StartTraining()
    {
        TrainingRunner selected = RunnerOptions.Options[RunnerSelection.value];
        selected = Instantiate(selected);
        DontDestroyOnLoad(selected);

        Type runner = selected.GetType();
        var variables = runner.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(field => Attribute.IsDefined(field, typeof(ShowInMenuAttribute)));

        foreach (var variable in variables)
        {
            if (variable.FieldType.IsPrimitive)
            {
                try 
                {
                    var input = primitiveInputs.Dequeue();

                    if (variable.FieldType == typeof(float) && float.TryParse(input.text, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
                        variable.SetValue(selected, result);
                    else if (variable.FieldType == typeof(int) && int.TryParse(input.text, out int result2))
                        variable.SetValue(selected, result2);
                }
                catch (Exception _e)
                {
                    Debug.LogError("Something went wrong while loading input");
                }
            }

        }

        if (attacker.activeInHierarchy)
            attacker.GetComponent<TrainingSettings>().InitializeVariables();

        if (defender.activeInHierarchy)
            defender.GetComponent<TrainingSettings>().InitializeVariables();

        SceneManager.LoadScene(2);
    }
}
