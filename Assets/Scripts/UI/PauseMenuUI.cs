using UnityEngine;
using UnityEngine.SceneManagement;

namespace SolarOdyssey.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;

        public void OpenPauseMenu()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }

        public void QuitGame()
        {
            Time.timeScale = 1f;

            Application.Quit();
        }
    }
}