using UnityEngine;

public class GrenadeCard : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab;
    public GameObject stingerPrefab;
    public int grenadeCount = 3;
    public int stingersPerGrenade = 5;
    public int stingerDamage = 2;
    public float arcHeight = 2f;
    public float travelTime = 0.8f;
    public KeyCode throwKey = KeyCode.Y;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(throwKey) && grenadeCount > 0)
            ThrowGrenade();
    }

    void ThrowGrenade()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCam.transform.position.z);
        Vector3 targetPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        targetPos.z = 0f;

        GameObject grenadeObj = Instantiate(grenadePrefab, transform.position, Quaternion.identity);

        GrenadeProjectile gp = grenadeObj.GetComponent<GrenadeProjectile>();
        if (gp != null)
        {
            gp.arcHeight = arcHeight;
            gp.travelTime = travelTime;
            gp.Launch(transform.position, targetPos, stingerPrefab, stingersPerGrenade, stingerDamage);
        }

        grenadeCount--;
        Debug.Log($"Grenade thrown! Grenades remaining: {grenadeCount}");
    }
}