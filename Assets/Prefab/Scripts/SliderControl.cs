using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A slider control.
/// </summary>
public class SliderControl : MonoBehaviour
{
    /// <summary>
    /// The slider control.
    /// <br />
    /// Note: The <see cref="ValueText"/> property and the corresponding property for this semantic
    /// value on <see cref="GpuGraph"/> must be connected to the <c>onValueChanged</c> event in
    /// order for this control to work properly.
    /// </summary>
    [SerializeField]
    Slider slider;

    /// <summary>
    /// The text object for displaying the slider value.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI valueText;

    /// <summary>
    /// The precision with which to display the slider value. This is the number of digits to show
    /// after the decimal point. A value of 0 does not show the decimal point.
    /// </summary>
    [SerializeField, Range(0, 6)]
    uint valueTextPrecision = 4;

    /// <summary>
    /// A public interface that sets the value of <see cref="valueText"/> with precision
    /// <see cref="valueTextPrecision"/>. This must be connected to the <c>onValueChanged</c> event
    /// of <see cref="slider"/>.
    /// </summary>
    public float Value
    {
        set { valueText.SetText(value.ToString("N" + valueTextPrecision)); }
    }
}
