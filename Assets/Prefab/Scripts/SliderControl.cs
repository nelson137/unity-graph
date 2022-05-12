using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderControl : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI valueText;

    [SerializeField, Range(0, 6)]
    uint valueTextPrecision = 4;

    public float Value
    {
        set { valueText.SetText(value.ToString("N" + valueTextPrecision)); }
    }
}
