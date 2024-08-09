/*using UnityEngine;
using UnityEngine.UI;


public class Counter : MonoBehaviour {
    private static Counter instance;

    private int count;
    [SerializeField] private Text countText;


    public static Counter Instance {
        get {
            if (instance == null) {
                instance = new Counter();
            }
            return instance;
        }
    }

    private Counter() {}

    public int GetCount() {
        return count;
    }

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
*/

using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private Text countText;
    private static Counter _instance;

    public static Counter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Counter>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("Counter");
                    _instance = singleton.AddComponent<Counter>();
                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }

    public int Points { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        Points += points;
        Debug.Log("Pontos: " + Points);
    }

    public void ResetPoints()
    {
        Points = 0;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Airplane")) {
            this.AddPoints(1);
            countText.text = "x " + Points;
            Destroy(other.gameObject);
        }
    }
}
