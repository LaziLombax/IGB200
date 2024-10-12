using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPin : MonoBehaviour
{
    public Slider barSlider;

    public void SetValueForBar(float value)
    {
        barSlider.value = value;
    }
}
