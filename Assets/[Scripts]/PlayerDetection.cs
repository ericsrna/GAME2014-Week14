using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header("Sensing Suite")]
    public LayerMask collisionLayerMask;
    public Collider2D colliderName;
    public ContactFilter2D contactFilter;
    public bool playerDetected;
    public bool LOS;
    public Transform playerTransform;
    public float playerDirection;
    public float enemyDirection;
    
    private Vector2 playerDirectionVector;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
        LOS = false;
        playerDetected = false;
        playerDirectionVector = Vector2.zero;
    }

    private void Update()
    {
        if (playerDetected)
        {
            //LOS = Physics2D.Linecast(transform.position, playerTransform.position, collisionLayerMask);
            var hits = Physics2D.Linecast(transform.position, playerTransform.position, collisionLayerMask);
            
            colliderName = hits.collider;

            playerDirectionVector = playerTransform.position - transform.position;
            playerDirection = (playerDirectionVector.x > 0 ? 1.0f : -1.0f);
            enemyDirection = GetComponentInParent<EnemyController>().direction.x;

            LOS = ((hits.collider.gameObject.name == "Player") && (playerDirection == enemyDirection));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (LOS) ? Color.green : Color.red;
        if (playerDetected)
            Gizmos.DrawLine(transform.position, playerTransform.position);

        Gizmos.color = (playerDetected) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 15);
    }
}
