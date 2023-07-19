using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CanvasScaleAdjust))]
public class CanvasScaleAdjustEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ratio"));

        if (GUILayout.Button("Adjust Scale"))
        {
            (target as CanvasScaleAdjust).Adjust();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public class CanvasScaleAdjust : MonoBehaviour
{
    [SerializeField] private float ratio;

    public void Adjust()
    {
        for (int i = 0; i < transform.childCount; i++) ChildrenDFS(transform.GetChild(i));
    }

    private void ChildrenDFS(Transform current)
    {
        current.localPosition *= ratio;
        if (current is RectTransform) (current as RectTransform).sizeDelta *= ratio;
        if (current.TryGetComponent<TextMeshProUGUI>(out var comp)) comp.fontSize *= ratio;
        for (int i = 0; i < current.childCount; i++) ChildrenDFS(current.GetChild(i));
    }
}
