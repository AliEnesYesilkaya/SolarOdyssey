using UnityEngine;

namespace SolarOdyssey
{
    public class GoldPickup : MonoBehaviour
    {
        [SerializeField] private int goldAmount = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Debug.Log("Altın alındı: +" + goldAmount);

            Destroy(gameObject);
        }
    }
}