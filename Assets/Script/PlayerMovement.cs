using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    [Header("Idle Sprites")]
    public Sprite idleDown;
    public Sprite idleUp;
    public Sprite idleSide;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float lastMoveX = 0f;
    private float lastMoveY = -1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ShowIdleSprite();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveY = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveY = -1f;

        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += movement * speed * Time.deltaTime;

        bool isMoving = moveX != 0 || moveY != 0;

        if (isMoving)
        {
            lastMoveX = moveX;
            lastMoveY = moveY;

            if (animator != null)
            {
                animator.enabled = true;
                animator.SetBool("isMoving", true);
            }

            if (spriteRenderer != null)
            {
                if (moveX < 0)
                    spriteRenderer.flipX = true;
                else if (moveX > 0)
                    spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
                animator.enabled = false;
            }

            ShowIdleSprite();
        }
    }

    void ShowIdleSprite()
    {
        if (spriteRenderer == null) return;

        if (Mathf.Abs(lastMoveX) > Mathf.Abs(lastMoveY))
        {
            if (idleSide != null)
                spriteRenderer.sprite = idleSide;

            if (lastMoveX < 0)
                spriteRenderer.flipX = true;
            else if (lastMoveX > 0)
                spriteRenderer.flipX = false;
        }
        else
        {
            if (lastMoveY > 0)
            {
                if (idleUp != null)
                    spriteRenderer.sprite = idleUp;

                spriteRenderer.flipX = false;
            }
            else
            {
                if (idleDown != null)
                    spriteRenderer.sprite = idleDown;

                spriteRenderer.flipX = false;
            }
        }
    }
}