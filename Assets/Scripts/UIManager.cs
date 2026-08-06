using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        if (pausePanel == null) return;
        bool isActive = pausePanel.activeSelf;
        pausePanel.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }
}
