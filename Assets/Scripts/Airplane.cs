using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Airplane : MonoBehaviour {
    private float scaleFactor = 0.5f;
    private int currentPointIndex;
    private bool isDragging, isGrounded;
    private Vector2 moveDirection;
    private Vector3 originalPosition, targetPosition;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPoints;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float speed, landSpeed;
    [SerializeField] private AudioClip explosionSound;

    void Start() {
        currentPointIndex = 0;
        isGrounded = false;
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
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = new Color(1f, 1f, 1f, 0.5f) };
    }

    void Update() {
        if (isGrounded) { return; }

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
            this.animator.SetBool("Destroyed", true);
            speed = 0;
            Invoke("GameOver", 2f);
        } else if (
            (other.CompareTag("LandingSite") && tag == "Airplane")
            || (other.CompareTag("HelicopterLandingSite") && tag == "Helicopter")
        ) {
            isGrounded = true;
            GameObject spawner = GameObject.FindGameObjectWithTag("PlaneSpawner");
            spawner.GetComponent<PlaneSpawner>().RemovePlane();
            GameController.Instance.AddPoints(1);
            GetComponent<Collider2D>().enabled = false;
            if (other.CompareTag("LandingSite")) {
                originalPosition = other.transform.position;
                Transform targetTransform = other.transform.Find("Target");
                targetPosition = targetTransform.position;
                StartCoroutine(this.LandingAnimation());
            } else {
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator LandingAnimation() {        
        // Update order in layer to 2
        spriteRenderer.sortingOrder = 2;

        // Rotate
        transform.position = originalPosition;
        Vector3 direction3D = targetPosition - originalPosition;
        Vector2 direction2D = new Vector2(direction3D.x, direction3D.y);
        SetDirection(direction2D);

        // Redimensionar o avião para simular descida
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;
        float resizeDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < resizeDuration) {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / resizeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        float moveDuration = 3f;
        elapsedTime = 0f;

        // Desacelerar até parar
        float originalSpeed = landSpeed;
        while (elapsedTime < moveDuration) {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            speed = Mathf.Lerp(originalSpeed, 0, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
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

    public void SetScaleFactor(float scaleFactor) {
        this.scaleFactor = scaleFactor;
    }
}
