using UnityEngine;

namespace SolarOdyssey.Core
{
    public class BossDeathEffect : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 1.2f;

        private void Start()
        {
            Destroy(
                gameObject,
                destroyTime
            );
        }
    }
}