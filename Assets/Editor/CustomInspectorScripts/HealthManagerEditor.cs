using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthManager))]
[CanEditMultipleObjects]
public class HealthManagerEditor : Editor
{
    HealthManager inspectedObject;

    public void OnEnable()
    {
        inspectedObject = (HealthManager)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("HP UI Display:");
        inspectedObject.HPDisplay = (PlayerHpUiDisplay)EditorGUILayout.ObjectField(inspectedObject.HPDisplay, typeof(PlayerHpUiDisplay), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("HP:");
        inspectedObject.HP = EditorGUILayout.FloatField(inspectedObject.HP);
        EditorGUILayout.EndHorizontal();
    }
}
