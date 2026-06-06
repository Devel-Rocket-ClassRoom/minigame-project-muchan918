#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SetRenderingLayerTool : EditorWindow
{
    [MenuItem("Tools/Set PreviewPlayer Rendering Layer")]
    public static void SetLayer()
    {
        var preview = GameObject.Find("PreviewPlayer");
        if (preview == null)
        {
            Debug.LogError("PreviewPlayer 없음");
            return;
        }

        foreach (var renderer in preview.GetComponentsInChildren<Renderer>())
        {
            renderer.renderingLayerMask = 2; // Layer 1
            EditorUtility.SetDirty(renderer);
        }

        Debug.Log("완료");
    }
}
#endif
