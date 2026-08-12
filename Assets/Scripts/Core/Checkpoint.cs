using UnityEngine;

namespace SolarOdyssey.Core
{
    public class Checkpoint : MonoBehaviour
    {
        public static Vector3 RespawnPosition;//oyuncunun yeniden doðacaðý son koordinat
        public static bool HasCheckpoint; //checkpointe ulaþtý mý 

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))//tag kontrolu 
            {
                RespawnPosition = transform.position;
                HasCheckpoint = true; //chechkpoint konumuna ulaþma

                Debug.Log("Checkpoint reached!");
            }
        }
    }
}