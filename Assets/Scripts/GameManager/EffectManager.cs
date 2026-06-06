using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [SerializeField]
    private GameObject animalHitEffect;

    [SerializeField]
    private GameObject animalDieEffect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayAnimalHit(Vector3 position)
    {
        if (animalHitEffect != null)
            Instantiate(animalHitEffect, position + Vector3.up * 0.5f, Quaternion.identity);
    }

    public void PlayAnimalDie(Vector3 position)
    {
        if (animalDieEffect != null)
            Instantiate(animalDieEffect, position + Vector3.up * 0.5f, Quaternion.identity);
    }
}
