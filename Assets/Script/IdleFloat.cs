using UnityEngine;

public class IdleFloat : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatAmount = 0.03f;

    private Animator animator;
    private Vector3 basePosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        basePosition = transform.position;
    }

    void Update()
    {
        bool isMoving = false;

        if (animator != null)
        {
            isMoving = animator.enabled && animator.GetBool("isMoving");
        }

        if (isMoving)
        {
            basePosition = transform.position;
            transform.position = basePosition;
        }
        else
        {
            float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.position = basePosition + new Vector3(0f, offsetY, 0f);
        }
    }
}