using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TMP_Text nameText;
    public SpriteRenderer artworkSprite;
    public GameObject equippedBadge;
    public GameObject equippedBadgeImage;

    [Header("UI Audio")]
    [SerializeField] private float transitionClickDelay = 0.1f;

    private int selectedOption = 0;
    private bool isTransitioning = false;

    void Start()
    {
        selectedOption = PlayerPrefs.GetInt("SelectedCharacter", 0);
        UpdateCharacter(selectedOption);
    }

    public void NextOption()
    {
        if (isTransitioning)
            return;

        PlayUIClick();

        selectedOption++;
        if (selectedOption >= characterDB.CharacterCount)
            selectedOption = 0;

        UpdateCharacter(selectedOption);
    }

    public void BackOption()
    {
        if (isTransitioning)
            return;

        PlayUIClick();

        selectedOption--;
        if (selectedOption < 0)
            selectedOption = characterDB.CharacterCount - 1;

        UpdateCharacter(selectedOption);
    }

    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);

        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;

        int savedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        bool isEquipped = selectedOption == savedIndex;

        if (equippedBadge != null)
            equippedBadge.SetActive(isEquipped);

        if (equippedBadgeImage != null)
            equippedBadgeImage.SetActive(isEquipped);
    }

    public void EquipSelection()
    {
        if (isTransitioning)
            return;

        PlayUIClick();

        PlayerPrefs.SetInt("SelectedCharacter", selectedOption);
        PlayerPrefs.Save();
        UpdateCharacter(selectedOption);
    }

    public void PlayGame()
    {
        if (isTransitioning)
            return;

        StartCoroutine(PlayGameRoutine());
    }

    public void GoToMainMenu()
    {
        if (isTransitioning)
            return;

        StartCoroutine(GoToMainMenuRoutine());
    }

    private IEnumerator PlayGameRoutine()
    {
        isTransitioning = true;

        PlayUIClick();
        yield return new WaitForSecondsRealtime(transitionClickDelay);

        SceneManager.LoadScene("Game-Scene");
    }

    private IEnumerator GoToMainMenuRoutine()
    {
        isTransitioning = true;

        PlayUIClick();
        yield return new WaitForSecondsRealtime(transitionClickDelay);

        SceneManager.LoadScene("Main_Menu");
    }

    private void PlayUIClick()
    {
        if (UIButtonAudio.Instance != null)
            UIButtonAudio.Instance.PlayClick();
    }
}