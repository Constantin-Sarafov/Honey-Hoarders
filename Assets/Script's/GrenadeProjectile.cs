using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public float travelTime = 0.8f;
    public float arcHeight = 2f;

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _elapsed = 0f;
    private bool _launched = false;

    private GameObject _stingerPrefab;
    private int _stingerCount;
    private int _stingerDamage;

    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.sortingLayerName = "Player";
        _sr.sortingOrder = 1;
    }

    public void Launch(Vector3 startPos, Vector3 targetPos, GameObject stingerPrefab, int stingerCount, int stingerDamage)
    {
        _startPos = startPos;
        _targetPos = targetPos;
        _stingerPrefab = stingerPrefab;
        _stingerCount = stingerCount;
        _stingerDamage = stingerDamage;
        _launched = true;
    }

    void Update()
    {
        if (!_launched) return;

        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / travelTime);

        Vector3 linearPos = Vector3.Lerp(_startPos, _targetPos, t);
        float arc = arcHeight * Mathf.Sin(t * Mathf.PI);
        transform.position = new Vector3(linearPos.x, linearPos.y + arc, linearPos.z);

        transform.Rotate(0f, 0f, -360f * Time.deltaTime);

        float scale = Mathf.Lerp(1.2f, 0.8f, t);
        transform.localScale = new Vector3(scale, scale, 1f);

        if (t >= 1f)
            Explode();
    }

    void Explode()
    {
        float angleStep = 360f / _stingerCount;

        for (int i = 0; i < _stingerCount; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject stinger = Instantiate(_stingerPrefab, transform.position, Quaternion.identity);

            BulletScript bs = stinger.GetComponent<BulletScript>();
            if (bs != null)
            {
                bs.SetDamage(_stingerDamage);
                bs.SetDirection(direction);
            }
        }

        Destroy(gameObject);
    }
}