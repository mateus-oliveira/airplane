using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Airplane : MonoBehaviour {
    private int currentPointIndex;
    private bool isDragging;
    private Vector2 moveDirection;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPoints;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private AudioClip explosionSound;

    void Start() {
        currentPointIndex = 0;
        isDragging = false;
        pathPoints = new List<Vector3>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Line Renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.sortingOrder = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.white };
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // Verifica se o toque está sobre o avião
            if (Vector2.Distance(transform.position, mousePos) < 0.5f) {
                isDragging = true;
                pathPoints.Clear();
                pathPoints.Add(transform.position);
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, transform.position);
            }
        }

        if (isDragging && Input.GetMouseButton(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (Vector3.Distance(pathPoints[pathPoints.Count - 1], mousePos) > 0.1f)
            {
                pathPoints.Add(mousePos);
                lineRenderer.positionCount = pathPoints.Count;
                lineRenderer.SetPosition(pathPoints.Count - 1, mousePos);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (this.isDragging){
                this.currentPointIndex = 0;
            }
            this.isDragging = false;
        }

        if (!isDragging && pathPoints.Count > 1) {
            this.FollowPath();
        }

        if (pathPoints.Count <= 0) {
            this.transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        string tag = this.gameObject.tag;
        if (other.CompareTag("Airplane") || other.CompareTag("Helicopter") || other.CompareTag("Ducks")) {
            AudioManager.instance.PlayAudio(explosionSound);
            animator.SetBool("Destroyed", true);
            speed = 0;
            Invoke("GameOver", 2f);
        } else if (
            (other.CompareTag("LandingSite") && tag == "Airplane")
            || (other.CompareTag("HelicopterLandingSite") && tag == "Helicopter")
        ) {
            Destroy(this.gameObject);
            GameObject spawner = GameObject.FindGameObjectWithTag("PlaneSpawner");
            spawner.GetComponent<PlaneSpawner>().RemovePlane();
            GameController.Instance.AddPoints(1);
        }
    }

    private void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    private void FollowPath() {
        Vector3 targetPosition = pathPoints[currentPointIndex];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Ajustando a rotação do sprite
        moveDirection = targetPosition - transform.position;
        this.Rotate();

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= pathPoints.Count)
            {
                pathPoints.Clear();
                lineRenderer.positionCount = 0;
            }
        }
    }

    private void Rotate() {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public void SetDirection(Vector2 direction) {
        moveDirection = direction.normalized;
        this.Rotate();
    }
}
