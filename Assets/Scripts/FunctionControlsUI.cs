using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static FunctionLibrary;

/// <summary>
/// Show function controls on the canvas.
/// </summary>
public class FunctionControlsUI : MonoBehaviour
{
    /// <summary>
    /// The graph.
    /// </summary>
    [SerializeField]
    GpuGraph graph;

    /// <summary>
    /// The outer-most panel that holds all controls.
    /// </summary>
    [SerializeField]
    RectTransform controlsPanel;

    /// <summary>
    /// The resolution slider control.
    /// </summary>
    [SerializeField]
    Slider resolutionSlider;

    /// <summary>
    /// The function dropdown control.
    /// </summary>
    [SerializeField]
    TMP_Dropdown functionDropdown;

    /// <summary>
    /// The divider line that separates the function controls from the function properties controls.
    /// </summary>
    [SerializeField]
    GameObject dividerLine;

    /// <summary>
    /// The parent of all function properties panels. Note that this is a child of
    /// <see cref="controlsPanel"/> to isolate the panels from the rest of the controls.
    /// </summary>
    [SerializeField]
    RectTransform functionPropertiesPanels;

    /// <summary>
    /// A dummy variable to get the variants of this enum through reflection.
    /// </summary>
    FunctionName funcName;

    /// <summary>
    /// The original size of <see cref="controlsPanel"/>. This is the base size of the panel without
    /// the divider or any function properties.
    /// </summary>
    Vector2 originalControlsPanelSize = Vector2.zero;

    /// <summary>
    /// The currently displayed function. This is used to un-display when switching functions.
    /// </summary>
    int currentFunction = -1;

    void Awake()
    {
        currentFunction = (int)graph.CurrentFunctionName;

        resolutionSlider.minValue = (float)GpuGraph.minResolution;
        resolutionSlider.maxValue = (float)GpuGraph.maxResolution;
        resolutionSlider.value = graph.Resolution;
        resolutionSlider.onValueChanged.AddListener(v => graph.Resolution = (int)v);

        var funcNames = Enum.GetNames(funcName.GetType());
        var options = funcNames.Select(name => new TMP_Dropdown.OptionData(name)).ToList();
        functionDropdown.ClearOptions();
        functionDropdown.AddOptions(options);
        functionDropdown.value = currentFunction;
        functionDropdown.onValueChanged.AddListener(index => OnChangeFunction(index));

        dividerLine.gameObject.SetActive(false);
        for (int i = 0; i < functionPropertiesPanels.childCount; i++)
        {
            functionPropertiesPanels.GetChild(i).gameObject.SetActive(false);
        }

        originalControlsPanelSize = controlsPanel.sizeDelta;
        UpdateFunctionControls(currentFunction);
    }

    /// <summary>
    /// Switch to another function. Set the graph to transition mode and update the
    /// function-specific properties controls. This is tiggered by a change of the dropdown value.
    /// </summary>
    /// <param name="index">The index of the function to which to switch. This can be cast to a
    /// <see cref="FunctionName"/>.</param>
    void OnChangeFunction(int index)
    {
        graph.SmoothTransitionTo((FunctionName)index);
        UpdateFunctionControls(index);
        currentFunction = index;
    }

    /// <summary>
    /// Update the function-specific properties controls. Hide the current props panel and show the
    /// one for <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index of the function to which to switch.</param>
    void UpdateFunctionControls(int index)
    {
        // Deactivate the props panel for the current function.
        if (currentFunction >= 0)
        {
            try
            {
                functionPropertiesPanels.GetChild(currentFunction).gameObject.SetActive(false);
            }
            catch (UnityException e)
            {
                Debug.LogError("func " + index + " has no controls panel");
                Debug.LogError(e);
            }
        }

        // Get the props panel for func index.
        Transform funcPropsPanel;
        try
        {
            funcPropsPanel = functionPropertiesPanels.GetChild(index);
        }
        catch (UnityException e)
        {
            Debug.LogError("func " + index + " has no controls panel");
            Debug.LogError(e);
            return;
        }

        // Get the RectTransform so we can update the size.
        RectTransform funcPropsPanelRT = funcPropsPanel.GetComponent<RectTransform>();
        if (!funcPropsPanelRT)
        {
            Debug.LogError("controls panel for func " + index + " has no RectTransform component");
            return;
        }

        if (funcPropsPanelRT.childCount > 0)
        {
            // Panel has props; adjust the view for the panel.
            Vector2 newSize = originalControlsPanelSize;
            newSize.y += funcPropsPanelRT.sizeDelta.y;
            controlsPanel.sizeDelta = newSize;
            dividerLine.SetActive(true);
            funcPropsPanelRT.gameObject.SetActive(true);
        }
        else
        {
            // Panel doesn't have any props; hide the divider and reset the size.
            dividerLine.SetActive(false);
            controlsPanel.sizeDelta = originalControlsPanelSize;
        }
    }
}
