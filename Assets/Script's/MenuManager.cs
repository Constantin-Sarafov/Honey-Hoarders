using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        if (UIButtonAudio.Instance != null)
            UIButtonAudio.Instance.PlayClickAndLoadScene("Game-Scene");
        else
            SceneManager.LoadScene("Game-Scene");
    }

    public void SkinsButton()
    {
        if (UIButtonAudio.Instance != null)
            UIButtonAudio.Instance.PlayClickAndLoadScene("Skin_Select");
        else
            SceneManager.LoadScene("Skin_Select");
    }

    public void QuitButton()
    {
        if (UIButtonAudio.Instance != null)
            UIButtonAudio.Instance.PlayClickAndQuit();
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}