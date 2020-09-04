using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(GroundCreator))]
public class CustomGroundCreatorInspector : Editor
{
    GroundCreator creator;
    SerializedProperty autoUpdate;
    SerializedProperty vegDensity;
    SerializedProperty treeDensity;
    SerializedProperty treeScaleRange;
    float old_vegDensity;
    float old_treeDensity;

    void OnEnable() 
    {
        creator = target as GroundCreator;
        autoUpdate = serializedObject.FindProperty("autoUpdate");
        vegDensity = serializedObject.FindProperty("vegDensity");
        treeDensity = serializedObject.FindProperty("treeDensity");
    }

    public override void OnInspectorGUI()
    {
        old_vegDensity = vegDensity.floatValue;
        old_treeDensity = treeDensity.floatValue;
        
        serializedObject.Update();

        DrawDefaultInspector();

        using (new EditorGUI.DisabledScope(autoUpdate.boolValue))
        {
            if(GUILayout.Button("Generate Land"))
            {
                creator.GenerateLand();
            }
            if(GUILayout.Button("Delete Land"))
            {
                creator.DeleteLand();
            }
        }

        if(vegDensity.floatValue != old_vegDensity && vegDensity.floatValue + treeDensity.floatValue > 1f)
        {
            treeDensity.floatValue = EditorGUILayout.FloatField("Tree Density", 1 - vegDensity.floatValue);
        }
        if(treeDensity.floatValue != old_treeDensity && treeDensity.floatValue + vegDensity.floatValue > 1f)
        {
            vegDensity.floatValue = EditorGUILayout.FloatField("Veg Density", 1 - treeDensity.floatValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
