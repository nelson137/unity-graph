using System;
using System.Linq;
using UnityEngine;
using TMPro;
using static FunctionLibrary;

public class FunctionControlsUI : MonoBehaviour
{
    [SerializeField]
    Graph graph;

    [SerializeField]
    TMP_Dropdown dropdown;

    FunctionName funcName;

    void Awake()
    {
        var funcNames = Enum.GetNames(funcName.GetType());
        var options = funcNames.Select(name => new TMP_Dropdown.OptionData(name)).ToList();
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.value = (int)graph.GetCurrentFunctionName();
        dropdown.onValueChanged.AddListener(index => graph.SmoothTransitionTo((FunctionName)index));
    }
}
