using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        int selectedOption = PlayerPrefs.GetInt("SelectedCharacter", 0);

        Character character = characterDB.GetCharacter(selectedOption);

        spriteRenderer.sprite = character.characterSprite;

        if (character.animatorController != null)
            animator.runtimeAnimatorController = character.animatorController;

        transform.localScale = character.scale;
    }
}