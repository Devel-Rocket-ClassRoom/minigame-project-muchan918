using UnityEditor;
using UnityEngine;

public class FixQuirkyMaterials : EditorWindow
{
    [MenuItem("Tools/Fix Quirky Materials to URP")]
    static void FixMaterials()
    {
        Shader urpShader = Shader.Find("Shader Graphs/SoftSurfaceGraph");

        if (urpShader == null)
        {
            Debug.LogError("SoftSurfaceGraph 셰이더를 찾을 수 없어요!");
            return;
        }

        string[] guids = AssetDatabase.FindAssets(
            "t:Material",
            new[] { "Assets/Imported/Quirky Series" }
        );
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            mat.shader = urpShader;
            mat.SetColor("_Color", new Color(188f / 255f, 188f / 255f, 188f / 255f));
            mat.SetFloat("_Emission", 0.5f);
            EditorUtility.SetDirty(mat);
            count++;
            Debug.Log($"변환됨: {path}");
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"총 {count}개 머티리얼 변환 완료!");
    }
}
