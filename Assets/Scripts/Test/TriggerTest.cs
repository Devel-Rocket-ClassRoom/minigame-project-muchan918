using System.Collections;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    [SerializeField]
    private GameObject spherePrefab;

    private void Start()
    {
        StartCoroutine(SpawnSphere());
    }

    private IEnumerator SpawnSphere()
    {
        yield return new WaitForSeconds(3f);

        // Cube 위치에 겹치게 스폰
        Instantiate(spherePrefab, transform.position, Quaternion.identity);
        Debug.Log("[TriggerTest] Sphere 스폰");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TriggerTest] OnTriggerEnter 동작 - {other.gameObject.name}");
    }
}
