using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorruptionBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;
    public Image fill;

    public void ResetCorruption()
    {
        slider.maxValue = 100;
        slider.value = 0;
        fill.color = gradient.Evaluate(1f); // 1f is the rightmost point of the gradient
    }

    public void SetCorruption(int corruption)
    {
        slider.value = corruption;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
