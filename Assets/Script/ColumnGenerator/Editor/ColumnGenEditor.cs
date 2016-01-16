using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColumnGen))]
public class ColumnGenEditor : Editor
{

    ColumnGen generator;

    public void OnEnable()
    {
        generator = (ColumnGen)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        DrawDefaultInspector();
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        if (GUILayout.Button("Build Column"))
        {
            generator.GenerateColumn();
        }
    }
}
