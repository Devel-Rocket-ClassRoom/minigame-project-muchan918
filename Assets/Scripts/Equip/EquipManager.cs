using System.Collections.Generic;
using UnityEngine;

namespace EquipSystem
{
    public class EquipManager : MonoBehaviour
    {
        public static EquipManager Instance { get; private set; }

        [Header("본 이름 설정")]
        [SerializeField]
        private string headBoneName = "Socket_Hat";

        [SerializeField]
        private string rightHandBoneName = "Socket_RightHand";

        [SerializeField]
        private string spineBoneName = "QuickRigCharacter2_Spine2"; // 등무기는 아직 소켓 없으니 그대로

        private readonly Dictionary<EquipSlotType, GameObject> _equipped = new();

        private void Awake()
        {
            Instance = this;
        }

        // 프리팹 넘기면 해당 슬롯에 장착 (기존 아이템 자동 교체)
        public void Equip(EquipSlotType slot, GameObject prefab)
        {
            UnEquip(slot);

            if (prefab == null)
                return;

            Transform parent = GetBoneTransform(slot);
            if (parent == null)
            {
                Debug.LogWarning($"[EquipManager] {slot} 슬롯의 본을 찾지 못했습니다.");
                return;
            }

            GameObject instance = Instantiate(prefab, parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            _equipped[slot] = instance;
        }

        // 해당 슬롯 해제
        public void UnEquip(EquipSlotType slot)
        {
            if (_equipped.TryGetValue(slot, out GameObject current) && current != null)
                Destroy(current);

            _equipped.Remove(slot);
        }

        // 전체 해제
        public void UnEquipAll()
        {
            foreach (var slot in System.Enum.GetValues(typeof(EquipSlotType)))
                UnEquip((EquipSlotType)slot);
        }

        // 현재 장착된 오브젝트 반환 (없으면 null)
        public GameObject GetEquipped(EquipSlotType slot)
        {
            _equipped.TryGetValue(slot, out GameObject obj);
            return obj;
        }

        private Transform GetBoneTransform(EquipSlotType slot)
        {
            string boneName = slot switch
            {
                EquipSlotType.Hat => headBoneName,
                EquipSlotType.WeaponRight => rightHandBoneName,
                EquipSlotType.WeaponBack => spineBoneName,
                _ => null, // 옷/바지/신발은 루트에 붙임
            };

            if (boneName == null)
                return this.transform;

            return FindBoneRecursive(this.transform, boneName);
        }

        private Transform FindBoneRecursive(Transform parent, string boneName)
        {
            if (parent.name == boneName)
                return parent;

            foreach (Transform child in parent)
            {
                Transform found = FindBoneRecursive(child, boneName);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
