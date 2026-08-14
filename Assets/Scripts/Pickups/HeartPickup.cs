using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey
{
    public class HeartPickup : MonoBehaviour
    {
        [SerializeField] private int healAmount = 15;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))//player tagı yoksa işlemi durdur
                return;

            Health health = other.GetComponent<Health>(); //can bileşenini çek 

            if (health != null)
            {
                if (health.IsFullHealth())// fulsa içinden geç 
                    return;

                health.Heal(healAmount);//değilse canı belirlenen miktar al nesneyi yok et 
                Destroy(gameObject);
            }
        }
    }
}