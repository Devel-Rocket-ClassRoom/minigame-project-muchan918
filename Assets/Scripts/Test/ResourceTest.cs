using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceTest : MonoBehaviour
{
    public GameObject Tree;
    private ResourceObject resourceObject;

    private void Start()
    {
        resourceObject = Tree.GetComponent<ResourceObject>();
    }

    private void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            resourceObject.TakeDamage(10);
        }
    }
}
