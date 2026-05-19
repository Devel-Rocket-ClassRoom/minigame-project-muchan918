using UnityEngine;
using UnityEngine.InputSystem;

public class WorldItem : MonoBehaviour
{
    public ItemAsset Asset;

    private void Start()
    {
        Asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(Asset.ItemID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Interactive UI 표시
            Debug.Log($"[DropItemObject] {Asset.Data.DisplayName} 근처에 왔습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Interactive UI 숨기기
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Keyboard.current.iKey.wasPressedThisFrame)
            PickUp();
    }

    private void PickUp()
    {
        // TODO: 인벤토리에 추가
        Debug.Log($"[DropItemObject] {Asset.Data.DisplayName} 아이템 습득!");
        Destroy(gameObject);
    }
}
