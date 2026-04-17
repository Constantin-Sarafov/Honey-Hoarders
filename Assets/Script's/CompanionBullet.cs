using UnityEngine;

public class CompanionBullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private CompanionAI owner;
    private float timer;
    private float damage;

    public void Init(CompanionAI companionAI, float bulletDamage)
    {
        owner = companionAI;
        damage = bulletDamage;
        timer = lifetime;
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnEnable()
    {
        timer = lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyAI enemy = col.GetComponent<EnemyAI>();
        if (enemy == null)
            enemy = col.GetComponentInParent<EnemyAI>();
        if (enemy == null)
            enemy = col.transform.root.GetComponentInChildren<EnemyAI>();
        if (enemy == null) return;

        enemy.enemyHealth -= damage;
        Debug.Log("Dealt " + damage + " damage to " + col.gameObject.name);
        Deactivate();
    }

    public void DeactivateBullet()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (owner != null)
            owner.ReturnToPool(gameObject);
        else
            gameObject.SetActive(false);
    }
}