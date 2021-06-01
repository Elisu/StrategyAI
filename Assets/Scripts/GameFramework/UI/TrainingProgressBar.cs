using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingProgressBar : MonoBehaviour
{
    public Slider progressBar;
    public Gradient gradient;
    public Image fill;

    public void SetGenerationCount(int generationCount)
    {
        progressBar.maxValue = generationCount;
        progressBar.value = 0;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetProgress(int generationNumber)
    {
        progressBar.value = progressBar.maxValue - generationNumber;
        fill.color = gradient.Evaluate(progressBar.normalizedValue);

    }
}
