using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Movement")]
    public float horizontalSpeed;
    public Transform groundPoint;
    public Transform headPoint;
    public Transform inFrontPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isObstacleInFront;
    public bool isGroundAhead;
    public bool isGrounded;
    public Vector2 direction;

    private void Start()
    {
        direction = Vector2.left;
    }

    void Update()
    {
        isObstacleInFront = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, groundLayerMask);
        isGroundAhead = Physics2D.Linecast(groundPoint.position, headPoint.position, groundLayerMask);
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);

        if (isGrounded && isGroundAhead)
        {
            Move();
        }

        if (!isGroundAhead || isObstacleInFront)
        {
            Flip();
        }
    }

    public void Move()
    {
        transform.position += new Vector3(direction.x * horizontalSpeed * Time.deltaTime, 0.0f);
    }

    public void Flip()
    {
        float x = transform.localScale.x * -1.0f;
        direction *= -1.0f;

        transform.localScale = new Vector3(x, 1.0f, 1.0f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
        Gizmos.DrawLine(groundPoint.position, inFrontPoint.position);
        Gizmos.DrawLine(groundPoint.position, headPoint.position);
    }
}
