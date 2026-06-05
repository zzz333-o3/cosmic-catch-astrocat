using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float range = 2f;
    private Vector3 startPosition;
    private bool movingRight = true;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (moveSpeed <= 0) return;

        float rightEdge = startPosition.x + range;
        float leftEdge = startPosition.x - range;

        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightEdge)
            {
                movingRight = false;
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftEdge)
            {
                movingRight = true;
                spriteRenderer.flipX = false;
            }
        }
    }

    void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSlimeClick();
            TakeDamageEffect();
        }
    }

    public void TakeDamageEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HitAnim());
    }

    private IEnumerator HitAnim()
    {
        transform.localScale = new Vector3(1.2f, 0.8f, 1f);
        float t = 0;
        while (t < 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t / 0.1f);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}
