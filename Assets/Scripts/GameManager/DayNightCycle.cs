using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private Light directionalLight;

    [SerializeField]
    private float brightDuration = 90f;

    [SerializeField]
    private float darkenDuration = 90f;

    [SerializeField]
    private float maxIntensity = 1f;

    [SerializeField]
    private float minIntensity = 0f;

    private float elapsedTime = 0f;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= brightDuration)
        {
            directionalLight.intensity = maxIntensity;
        }
        else
        {
            float t = (elapsedTime - brightDuration) / darkenDuration;
            directionalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, Mathf.Clamp01(t));
        }
    }

    public void SetMorning()
    {
        elapsedTime = 0f;
        directionalLight.intensity = maxIntensity;
    }
}
