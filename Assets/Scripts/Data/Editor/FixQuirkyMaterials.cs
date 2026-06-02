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

        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Quirky Series" });
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

    [MenuItem("Tools/Fix iPoly3D Materials to URP")]
    static void FixiPolyMaterials()
    {
        Shader urpShader = Shader.Find("Universal Render Pipeline/Simple Lit");
        if (urpShader == null)
        {
            Debug.LogError("URP Simple Lit 셰이더를 찾을 수 없어요!");
            return;
        }

        // 머티리얼명 → 텍스처명 매핑
        var texMap = new System.Collections.Generic.Dictionary<string, string>
        {
            { "Collectibles_Material", "Collectibles_Texture" },
            { "Arrows_Material", "Collectibles_Texture" },
            { "Currency_Icons_Material", "Collectibles_Texture" },
            { "Icons_Material", "Collectibles_Texture" },
            { "Runes_Icons_Material", "Runes_Texture" },
            { "Runes_Stones_Material", "Runes_Texture" },
            { "Zodiact_Icons_Material", "Zodiac_Texture" },
            { "Zodiact_Icons_2", "Zodiac_Texture" },
            { "Lowercase_Alphabet_Material", "Collectibles_Texture" },
            { "Uppercase_Alphabet_Material", "Collectibles_Texture" },
            { "Numers_Material", "Collectibles_Texture" },
        };

        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/iPoly3D" });
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            mat.shader = urpShader;
            mat.SetColor("_BaseColor", Color.white);

            if (texMap.TryGetValue(mat.name, out string texName))
            {
                string[] texGuids = AssetDatabase.FindAssets(
                    texName + " t:Texture2D",
                    new[] { "Assets/iPoly3D" }
                );
                if (texGuids.Length > 0)
                {
                    string texPath = AssetDatabase.GUIDToAssetPath(texGuids[0]);
                    Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
                    mat.SetTexture("_BaseMap", tex);
                    Debug.Log($"변환됨: {mat.name} → {texName}");
                }
            }
            else
            {
                Debug.LogWarning($"매핑 없음: {mat.name}");
            }

            EditorUtility.SetDirty(mat);
            count++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"총 {count}개 머티리얼 변환 완료!");
    }
}
