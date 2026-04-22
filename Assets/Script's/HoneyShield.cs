using UnityEngine;
using TMPro;

public class HoneyShield : MonoBehaviour
{
    [Header("Settings")]
    public bool isActive = false;
    [Range(0, 100)] public float blockPercentage = 0;

    [Header("Visual Settings")]
    [SerializeField] private GameObject shieldVisualPrefab;
    [SerializeField] private float flickerDuration = 0.2f;
    [SerializeField] private Vector3 shieldScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("UI")]
    public TMP_Text shieldText;

    private GameObject spawnedShield;

    void Awake()
    {
        if (shieldVisualPrefab != null && spawnedShield == null)
        {
            spawnedShield = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity, transform);
            spawnedShield.transform.localScale = shieldScale;
            spawnedShield.SetActive(false);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    void OnValidate()
    {
        if (!Application.isPlaying) return;

        if (isActive && spawnedShield == null && shieldVisualPrefab != null)
        {
            spawnedShield = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity, transform);
            spawnedShield.transform.localScale = shieldScale;
            spawnedShield.SetActive(false);
        }

        UpdateUI();
    }

    public void InitializeShield(float initialPercent)
    {
        isActive = true;
        blockPercentage = initialPercent;
        UpdateUI();
        Debug.Log($"<color=yellow>Shield Activated!</color> Chance: {blockPercentage}%");
    }

    public void UpgradeShield(float percentIncrease)
    {
        blockPercentage = Mathf.Clamp(blockPercentage + percentIncrease, 0, 100);
        UpdateUI();
        Debug.Log($"<color=cyan>Shield Upgraded!</color> New Chance: {blockPercentage}%");
    }

    public bool TryBlock()
    {
        if (!isActive) return false;

        float roll = Random.Range(0f, 100f);

        if (roll < blockPercentage)
        {
            TriggerShieldVisual();
            Debug.Log($"<color=green>SUCCESS!</color> Rolled {roll} against {blockPercentage}%");
            return true;
        }

        Debug.Log($"<color=red>FAILED!</color> Rolled {roll} against {blockPercentage}%");
        return false;
    }

    private void TriggerShieldVisual()
    {
        if (spawnedShield != null)
        {
            spawnedShield.transform.localScale = shieldScale;
            StopAllCoroutines();
            StartCoroutine(ShieldFlashRoutine());
        }
    }

    private System.Collections.IEnumerator ShieldFlashRoutine()
    {
        spawnedShield.SetActive(true);
        yield return new WaitForSeconds(flickerDuration);
        spawnedShield.SetActive(false);
    }

    public void UpdateUI()
    {
        if (shieldText != null)
        {
            shieldText.gameObject.SetActive(isActive);
            shieldText.text = $"Shield: {blockPercentage:F0}%";
        }
    }
}