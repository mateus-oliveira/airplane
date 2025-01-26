using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLight : MonoBehaviour
{
    [SerializeField] private float changeDuration, periodDuration;
    private Light sunLight;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = GetComponent<Light>();
        sunLight.intensity = Random.Range(0, 2);
        StartCoroutine(ChangeLightIntensity(sunLight.intensity == 1f ? 0f : 1f));
    }

    // Coroutine to change light intensity over time
    private IEnumerator ChangeLightIntensity(float targetIntensity)
    {
        yield return new WaitForSeconds(periodDuration);
    
        float startIntensity = sunLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < changeDuration)
        {
            sunLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / changeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sunLight.intensity = targetIntensity;
        StartCoroutine(ChangeLightIntensity(sunLight.intensity == 1f ? 0f : 1f));
    }
}