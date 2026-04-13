using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game-Scene");
    }

    public void OpenSkins()
    {
        SceneManager.LoadScene("SkinSelect");
    }
}