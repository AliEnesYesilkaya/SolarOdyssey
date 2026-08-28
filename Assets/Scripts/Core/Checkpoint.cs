using UnityEngine;

namespace SolarOdyssey.Core
{
    public class Checkpoint : MonoBehaviour
    {
        public static Vector3 RespawnPosition;
        public static bool HasCheckpoint;

        private void OnTriggerEnter2D(
            Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            RespawnPosition =
                transform.position;

            HasCheckpoint = true;

            // Checkpoint bilgisini kalıcı olarak kaydet.
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveCheckpoint(
                    transform.position
                );
            }

            // Checkpoint sesi
            if (Audio.EnvironmentAudio.Instance != null)
            {
                Audio.EnvironmentAudio.Instance.PlayCheckpoint();
            }

            Debug.Log(
                "Checkpoint reached!"
            );
        }
    }
}