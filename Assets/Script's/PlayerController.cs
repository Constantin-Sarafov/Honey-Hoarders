using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private Animator anim;

    [Header("Damage")]
    [SerializeField] private float invulnerabilityTime = 0.75f;

    [Header("Movement Audio")]
    [SerializeField] private AudioSource moveAudio;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("EXP Magnet")]
    public bool magnetActive = false;
    public float magnetRadius = 5f;
    public float magnetPullSpeed = 8f;

    private float targetVolume = 0f;
    private float baseVolume = 0f;

    private Rigidbody2D rb;
    private HealthManager healthManager;
    private PlayerDamageIndicator damageIndicator;
    private HoneyShield honeyShield;

    private Vector2 movement;
    private bool isInvulnerable = false;
    private bool isDead = false;

    private Coroutine invulnerabilityCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<HealthManager>();
        damageIndicator = GetComponent<PlayerDamageIndicator>();
        honeyShield = GetComponent<HoneyShield>();

        if (anim == null)
            anim = GetComponent<Animator>();

        magnetActive = false;

        if (moveAudio != null)
        {
            baseVolume = moveAudio.volume;
            moveAudio.loop = true;
            moveAudio.volume = 0f;

            if (!moveAudio.isPlaying)
                moveAudio.Play();
        }
    }

    void Update()
    {
        if (isDead)
        {
            movement = Vector2.zero;
            targetVolume = 0f;
            FadeMovementAudio();
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movement = new Vector2(moveX, moveY);

        bool isMoving = movement.sqrMagnitude > 0.01f;
        targetVolume = isMoving ? baseVolume : 0f;

        FadeMovementAudio();

        if (anim != null)
        {
            anim.SetFloat("Speed", movement.magnitude);

            if (Input.GetMouseButtonDown(0))
                anim.SetTrigger("Attack");

            if (Input.GetKeyDown(KeyCode.R))
                anim.SetTrigger("Reload");

            if (Input.GetKeyDown(KeyCode.H))
                anim.SetTrigger("Hurt");
        }

        if (Input.GetKeyDown(KeyCode.P))
            TakePlayerDamage(1);

        if (magnetActive)
            PullExpOrbs();
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, 10f * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead)
            return;

        if (other.CompareTag("Enemy"))
            TakePlayerDamage(1);
    }

    public void TakePlayerDamage(int amount)
    {
        if (!CanTakeDamage() || healthManager == null)
            return;

        if (honeyShield != null && honeyShield.isActive && honeyShield.TryBlock())
        {
            isInvulnerable = true;
            if (invulnerabilityCoroutine != null)
                StopCoroutine(invulnerabilityCoroutine);
            invulnerabilityCoroutine = StartCoroutine(InvulnerabilityRoutine());
            return;
        }

        isInvulnerable = true;

        healthManager.TakeDamage(amount, true);

        if (healthManager.IsDead())
        {
            invulnerabilityCoroutine = null;
            return;
        }

        if (anim != null)
            anim.SetTrigger("Hurt");

        if (damageIndicator != null)
            damageIndicator.ShowDamageBlink();

        if (invulnerabilityCoroutine != null)
            StopCoroutine(invulnerabilityCoroutine);

        invulnerabilityCoroutine = StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
        invulnerabilityCoroutine = null;
    }

    public bool CanTakeDamage()
    {
        return !isDead && !isInvulnerable && healthManager != null && !healthManager.IsDead();
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void HandleDeath()
    {
        if (isDead)
            return;

        isDead = true;
        isInvulnerable = true;
        movement = Vector2.zero;
        targetVolume = 0f;

        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
            invulnerabilityCoroutine = null;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Hurt");
        }
    }

    public void ActivateMagnet(float radius, float speed)
    {
        magnetActive = true;
        magnetRadius = radius;
        magnetPullSpeed = speed;
        Debug.Log($"Magnet activated! Radius: {radius}, Speed: {speed}");
    }

    public void UpgradeMagnet(float radiusIncrease, float speedIncrease)
    {
        magnetRadius += radiusIncrease;
        magnetPullSpeed += speedIncrease;
        Debug.Log($"Magnet upgraded! New Radius: {magnetRadius}, New Speed: {magnetPullSpeed}");
    }

    private void PullExpOrbs()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<ExpPickup>(out ExpPickup pickup))
                pickup.BeginPullToward(transform, magnetPullSpeed);
        }
    }

    private void FadeMovementAudio()
    {
        if (moveAudio == null)
            return;

        moveAudio.volume = Mathf.MoveTowards(
            moveAudio.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (!magnetActive) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}