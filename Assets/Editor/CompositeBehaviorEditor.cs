using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CompositeBehavior))]
public class CompositeBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var compositeBehavior = (CompositeBehavior)target;


        if (compositeBehavior.behaviors == null || compositeBehavior.behaviors.Count == 0)
        {
            _ = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            _ = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            EditorGUILayout.LabelField("Behaviors");
            EditorGUILayout.LabelField("Weights", GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            _ = EditorGUILayout.BeginVertical();
            for (int i = 0; i < compositeBehavior.behaviors.Count; i += 1)
            {
                BoidBehavior behavior = compositeBehavior.behaviors[i];
                float weight = compositeBehavior.weights[i];

                _ = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(20));
                compositeBehavior.behaviors[i] = (BoidBehavior)EditorGUILayout.ObjectField(behavior, typeof(BoidBehavior), false);
                compositeBehavior.weights[i] = EditorGUILayout.FloatField(weight, GUILayout.Width(50));

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(compositeBehavior);
            }
        }

        GUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

        _ = EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add behavior", GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.9f)))
        {
            AddBehavior(compositeBehavior);
            EditorUtility.SetDirty(compositeBehavior);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        _ = EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (compositeBehavior.behaviors != null && compositeBehavior.behaviors.Count > 0)
        {
            if (GUILayout.Button("Remove behavior"))
            {
                RemoveBehavior(compositeBehavior);
                EditorUtility.SetDirty(compositeBehavior);
            }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    void AddBehavior(CompositeBehavior cb)
    {
        cb.behaviors.Add(null);
        cb.weights.Add(1f);
    }

    void RemoveBehavior(CompositeBehavior cb)
    {
        cb.behaviors.RemoveAt(cb.behaviors.Count - 1);
        cb.weights.RemoveAt(cb.weights.Count - 1);
    }
}
