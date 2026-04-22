using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expAmount = 2;
    private Transform target;
    private bool isBeingPulled = false;
    private float pullSpeed;
    private EXPManager mgr;

    private void Start()
    {
        mgr = FindFirstObjectByType<EXPManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (mgr == null)
                mgr = other.GetComponent<EXPManager>();

            Collect();
        }
    }

    public void BeginPullToward(Transform playerTransform, float speed)
    {
        if (isBeingPulled) return;
        target = playerTransform;
        pullSpeed = speed;
        isBeingPulled = true;
    }

    private void Update()
    {
        if (isBeingPulled && target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                pullSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, target.position) < 0.2f)
                Collect();
        }
    }

    private void Collect()
    {
        if (mgr != null)
            mgr.GainExperience(expAmount);
        else
            Debug.LogWarning("EXPManager not found in scene!");

        Destroy(gameObject);
    }
}