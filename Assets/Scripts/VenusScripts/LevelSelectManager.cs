using UnityEngine;
using UnityEngine.SceneManagement;

namespace SolarOdyssey.UI
{
    public class LevelSelectManager : MonoBehaviour
    {
        public void LoadEarth()
        {
            SceneManager.LoadScene("EarthLevel");
        }

        public void LoadVenus()
        {
            SceneManager.LoadScene("VenusLevel");
        }

        public void LoadSun()
        {
            SceneManager.LoadScene("SunLevel");
        }

        public void ReturnToLevelSelect()
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }
}