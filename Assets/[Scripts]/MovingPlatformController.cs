using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public PlatformDirection direction;

    [Range(0f, 20f)] public float horizontalDistance = 8.0f;
    [Range(0f, 20f)] public float horizontalSpeed = 1.0f;
    [Range(0f, 20f)] public float verticalDistance = 0.0f;
    [Range(0f, 20f)] public float verticalSpeed = 1.0f;
    
    private Vector2 startPoint;

    void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        switch (direction)
        {
            case PlatformDirection.HORIZONTAL:
                transform.position = new Vector3(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    startPoint.y, 0.0f);
                break;
            case PlatformDirection.VERTICAL:
                transform.position = new Vector3(
                    startPoint.x,
                    Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPoint.y,
                    0.0f);
                break;
            case PlatformDirection.DIAGNAL_UP:
                transform.position = new Vector3(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPoint.y,
                    0.0f);
                break;
            case PlatformDirection.DIAGONAL_DOWN:
                transform.position = new Vector3(
                    Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPoint.x,
                    startPoint.y - Mathf.PingPong(verticalSpeed * Time.time, verticalDistance),
                    0.0f);
                break;
        }
    }
}
