using UnityEngine;

public class WorldItemManager : MonoBehaviour
{
    public static WorldItemManager Instance { get; private set; }
    public GameObject WorldItemPrefab;

    private void Awake() => Instance = this;
}
