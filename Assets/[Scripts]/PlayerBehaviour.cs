using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Movement")]
    public float airFactor;
    public float verticalForce;
    public float horizontalForce;
    public float horizontalSpeed;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    [Header("Animations")]
    public Animator animator;
    public PlayerAnimationState animationState;

    [Header("Controls")]
    public Joystick leftJosystick;
    [Range(0.1f, 1f)]
    public float verticalThreshold;

    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        leftJosystick = GameObject.Find("Left Joystick").GetComponent<Joystick>();
    }

    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        Move();
        Jump();
        AirCheck();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") + leftJosystick.Horizontal;

        if (x != 0.0f)
        {
            Flip(x);

            rigidbody2D.AddForce(Vector2.right * horizontalForce * ((x > 0.0) ? 1.0f : -1.0f) * ((isGrounded) ? 1 : airFactor));

            float clampedX = Mathf.Clamp(rigidbody2D.velocity.x, -horizontalSpeed, horizontalSpeed);
            rigidbody2D.velocity = new Vector3(clampedX, rigidbody2D.velocity.y);

            ChangeAnimation(PlayerAnimationState.RUN);
        }

        if ((isGrounded) && (x == 0))
        {
            ChangeAnimation(PlayerAnimationState.IDLE);
        }
    }

    private void ChangeAnimation(PlayerAnimationState state)
    {
        animationState = state;
        animator.SetInteger("AnimationState", (int)animationState);
    }

    private void Flip(float x)
    {
        if (x != 0.0f)
        {
            transform.localScale = new Vector3((x > 0.0f) ? 1.0f : -1.0f, 1.0f);
        }
    }

    private void Jump()
    {
        var y = Input.GetAxis("Jump") + leftJosystick.Vertical;

        if (isGrounded && y > verticalThreshold)
        {
            rigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }
    }

    private void AirCheck()
    {
        if (!isGrounded)
        {
            ChangeAnimation(PlayerAnimationState.JUMP);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }
}
