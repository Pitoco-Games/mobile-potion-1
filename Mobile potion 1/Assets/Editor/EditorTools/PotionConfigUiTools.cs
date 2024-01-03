using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PotionConfig))]
public class PotionConfigUiTools : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var potionConfig = (PotionConfig) target;
        if(GUILayout.Button("Sort ingredients list", GUILayout.Height(40)))
        {
            potionConfig.SortIngredientsList();
        }
    }
}