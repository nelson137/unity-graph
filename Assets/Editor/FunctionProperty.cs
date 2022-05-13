using UnityEngine;
using UnityEditor;

public class FunctionProperty : MonoBehaviour
{
    [MenuItem("GameObject/Graph Properties/Slider", false, 0)]
    static void CreateFuncPropSlider()
    {
        var newGO = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/Prefab/Prop Panel - Slider.prefab"
        );
        if (!newGO)
        {
            EditorUtility.DisplayDialog("Error", "Failed to find slider prefab.", "OK");
            return;
        }

        var selected = Selection.activeGameObject;
        if (selected)
        {
            Instantiate(newGO, selected.transform);
        }
        else
        {
            Instantiate(newGO);
        }
    }
}
