using UnityEngine;
using UnityEngine.UI;


public class Counter : MonoBehaviour {
    private int count;
    [SerializeField] private Text countText;

    void Start() {
        count = 0;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Airplane")) {
            count++;
            countText.text = "x " + count;
            Destroy(other.gameObject);
        }
    }
}
