using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffects : MonoBehaviour
{
    [SerializeField] private bool on;   
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("YellowLight")){
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            GetComponent<Light>().enabled = on;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Light>().enabled = !on;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
