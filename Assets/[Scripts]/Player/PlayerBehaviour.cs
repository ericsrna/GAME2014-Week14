using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Dust Trail")]
    public ParticleSystem dustTrail;
    public Color dustTrailColour;

    [Header("Screen Shake Properties")]
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float shakeduration;
    public float shakeTimer;
    public bool isCameraShaking;

    [Header("Health System")]
    public HealthBarController health;
    public LifeCounterController life;
    public DeathPlaneController deathPlane;

    [Header("Controls")]
    public Joystick leftJosystick;
    [Range(0.1f, 1f)]
    public float verticalThreshold;

    private Rigidbody2D rigidbody2D;
    private SoundManager soundManager;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dustTrail = GetComponentInChildren<ParticleSystem>();
        if (GameObject.Find("OnScreenControls"))
        {
            leftJosystick = GameObject.Find("Left Joystick").GetComponent<Joystick>();
        }
        health = FindObjectOfType<PlayerHealth>().GetComponent<HealthBarController>();
        life = FindObjectOfType<LifeCounterController>();
        deathPlane = FindObjectOfType<DeathPlaneController>();
        soundManager = FindObjectOfType<SoundManager>();

        // Camera Shake
        isCameraShaking = false;
        shakeTimer = shakeduration;
        virtualCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void Update()
    {
        if (health.value <= 0)
        {
            life.LoseLife();

            if (life.lifeValue > 0)
            {
                health.ResetHealth();
                deathPlane.ReSpawn(gameObject);
                soundManager.PlaySoundFX(Sound.DEATH, Channel.PLAYER_DEATH_FX);
            }
        }

        if (life.lifeValue <= 0)
        {
            SceneManager.LoadScene("End");
        }

    }

    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        Move();
        Jump();
        AirCheck();

        // Camera Shake Control
        if (isCameraShaking)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f && perlin != null) // timed out
            {
                perlin.m_AmplitudeGain = 0.0f;
                shakeTimer = shakeduration;
                isCameraShaking = false;
            }
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") + (GameObject.Find("OnScreenControls") ? leftJosystick.Horizontal : 0.0f);

        if (x != 0.0f)
        {
            Flip(x);

            rigidbody2D.AddForce(Vector2.right * horizontalForce * ((x > 0.0) ? 1.0f : -1.0f) * ((isGrounded) ? 1 : airFactor));

            float clampedX = Mathf.Clamp(rigidbody2D.velocity.x, -horizontalSpeed, horizontalSpeed);
            rigidbody2D.velocity = new Vector3(clampedX, rigidbody2D.velocity.y);

            ChangeAnimation(PlayerAnimationState.RUN);

            if (isGrounded)
            {
                CreateDustTrail();
            }
        }

        if ((isGrounded) && (x == 0))
        {
            ChangeAnimation(PlayerAnimationState.IDLE);
        }
    }

    private void CreateDustTrail()
    {
        dustTrail.GetComponent<Renderer>().material.SetColor("_Color", dustTrailColour);
        dustTrail.Play();
    }

    private void ShakeCamera()
    {
        if (perlin != null)
        {
            perlin.m_AmplitudeGain = shakeIntensity;
            isCameraShaking = true;
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
        float y = Input.GetAxis("Jump") + (GameObject.Find("OnScreenControls") ? leftJosystick.Vertical : 0.0f);

        if (isGrounded && y > verticalThreshold)
        {
            rigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            soundManager.PlaySoundFX(Sound.JUMP, Channel.PLAYER_SOUND_FX);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(20);

            if (life.lifeValue > 0)
            {
                soundManager.PlaySoundFX(Sound.HURT, Channel.PLAYER_HURT_FX);
            }
            ShakeCamera();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            health.TakeDamage(30);

            if (life.lifeValue > 0)
            {
                soundManager.PlaySoundFX(Sound.HURT, Channel.PLAYER_HURT_FX);
            }
            ShakeCamera();
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            health.TakeDamage(10);

            if (life.lifeValue > 0)
            {
                soundManager.PlaySoundFX(Sound.HURT, Channel.PLAYER_HURT_FX);
            }
            ShakeCamera();
        }
    }
}
