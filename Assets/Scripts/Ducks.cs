using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ducks : MonoBehaviour
{
    [SerializeField] private float speed, margin;
    private Vector2 direction;
    private SpriteRenderer sprite; 

    void Start()
    {
        // Determine the direction based on the initial position
        if (transform.position.x < 0)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        // Flip the sprite if moving right
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = direction == Vector2.right;
    }

    void Update()
    {
        // Move the duck
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if the duck is next to the border of the screen
        if ((direction == Vector2.right && transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - margin) ||
            (direction == Vector2.left && transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + margin))
        {
            PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
            collider.enabled = false;
        }

        // Check if the duck is next to the border of the screen
        if ((direction == Vector2.right && transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x) ||
            (direction == Vector2.left && transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Airplane") || other.CompareTag("Helicopter"))
        {
            Destroy(gameObject);
        }
    }
}