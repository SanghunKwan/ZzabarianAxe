using UnityEngine;

public class ClosingWindow : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
