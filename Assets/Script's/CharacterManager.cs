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

<<<<<<< Updated upstream
    private int selectedOption = 0;
=======
    [Header("UI Audio")]
    [SerializeField] private float transitionClickDelay = 0.1f;

    private int selectedOption = 0;
    private bool isTransitioning = false;
>>>>>>> Stashed changes

    void Start()
    {
        selectedOption = PlayerPrefs.GetInt("SelectedCharacter", 0);
        UpdateCharacter(selectedOption);
    }

    public void NextOption()
    {
<<<<<<< Updated upstream
        selectedOption++;
        if (selectedOption >= characterDB.CharacterCount) selectedOption = 0;
=======
        if (isTransitioning)
            return;

        PlayUIClick();

        selectedOption++;
        if (selectedOption >= characterDB.CharacterCount)
            selectedOption = 0;

>>>>>>> Stashed changes
        UpdateCharacter(selectedOption);
    }

    public void BackOption()
    {
<<<<<<< Updated upstream
        selectedOption--;
        if (selectedOption < 0) selectedOption = characterDB.CharacterCount - 1;
=======
        if (isTransitioning)
            return;

        PlayUIClick();

        selectedOption--;
        if (selectedOption < 0)
            selectedOption = characterDB.CharacterCount - 1;

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        if (isTransitioning)
            return;

        PlayUIClick();

>>>>>>> Stashed changes
        PlayerPrefs.SetInt("SelectedCharacter", selectedOption);
        PlayerPrefs.Save();
        UpdateCharacter(selectedOption);
    }

    public void PlayGame()
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("Game-Scene");
=======
        if (isTransitioning)
            return;

        StartCoroutine(PlayGameRoutine());
>>>>>>> Stashed changes
    }

    public void GoToMainMenu()
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("Main_Menu");
    }
=======
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
>>>>>>> Stashed changes
}