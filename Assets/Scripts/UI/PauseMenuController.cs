using UnityEngine;
using UnityEngine.SceneManagement;

namespace SolarOdyssey.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;

        private bool isPaused;

        private void Start()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }

        public void Quit()
        {
            Time.timeScale = 1f;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}