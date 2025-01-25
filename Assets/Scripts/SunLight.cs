using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLight : MonoBehaviour
{
    private Light sunLight;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = GetComponent<Light>();
        sunLight.intensity = Random.Range(0f, 1f);
        StartCoroutine(AlternateLightIntensity());
    }

    // Coroutine to alternate light intensity
    private IEnumerator AlternateLightIntensity()
    {
        while (true)
        {
            yield return StartCoroutine(ChangeLightIntensity(1f, 150f));
            yield return StartCoroutine(ChangeLightIntensity(0f, 150f));
        }
    }

    // Coroutine to change light intensity over time
    private IEnumerator ChangeLightIntensity(float targetIntensity, float duration)
    {
        float startIntensity = sunLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            sunLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sunLight.intensity = targetIntensity;
    }
}