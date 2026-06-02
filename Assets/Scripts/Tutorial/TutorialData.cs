using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Scriptable Objects/TutorialData")]
public class TutorialData : ScriptableObject
{
    public string title;
    public Sprite image;

    [TextArea]
    public string description;
}
