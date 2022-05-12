using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static FunctionLibrary;

public class FunctionControlsUI : MonoBehaviour
{
    [SerializeField]
    GpuGraph graph;

    [SerializeField]
    RectTransform controlsPanel;

    [SerializeField]
    Slider resolutionSlider;

    [SerializeField]
    TMP_Dropdown functionDropdown;

    [SerializeField]
    GameObject dividerLine;

    [SerializeField]
    RectTransform functionPropertiesPanels;

    FunctionName funcName;

    Vector2 originalControlsPanelSize = Vector2.zero;
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

    void OnChangeFunction(int index)
    {
        graph.SmoothTransitionTo((FunctionName)index);
        UpdateFunctionControls(index);
        currentFunction = index;
    }

    void UpdateFunctionControls(int index)
    {
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

        RectTransform funcPropsPanelRT = funcPropsPanel.GetComponent<RectTransform>();
        if (!funcPropsPanelRT)
        {
            Debug.LogError("controls panel for func " + index + " has no RectTransform component");
            return;
        }

        if (funcPropsPanelRT.childCount > 0)
        {
            Vector2 newSize = originalControlsPanelSize;
            newSize.y += funcPropsPanelRT.sizeDelta.y;
            controlsPanel.sizeDelta = newSize;
            dividerLine.SetActive(true);
            funcPropsPanelRT.gameObject.SetActive(true);
        }
        else
        {
            dividerLine.SetActive(false);
            controlsPanel.sizeDelta = originalControlsPanelSize;
        }
    }
}
