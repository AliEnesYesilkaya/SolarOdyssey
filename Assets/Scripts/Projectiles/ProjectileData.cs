using UnityEngine;

namespace SolarOdyssey.Projectiles 
{
    [CreateAssetMenu(
        fileName = "ProjectileData",
        menuName = "SolarOdyssey/Projectile Data"
    )]
    public class ProjectileData : ScriptableObject
    {
        [Header("Projectile Settings")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 5f;

        [Header("Projectile Visual")]
        [SerializeField] private Sprite projectileSprite;

        public int Damage => damage;
        public float Speed => speed;
        public float Lifetime => lifetime;
        public Sprite ProjectileSprite => projectileSprite;
    }
}