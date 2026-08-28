using System.Collections;
using SolarOdyssey.Combat;
using SolarOdyssey.Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolarOdyssey.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMovement PlayerMovement;

        private InputAction moveAction;
        private InputAction jumpAction;

        private Animator animator;
        private AttackHitbox attackHitbox;
        private PlayerAudio playerAudio;
        private PlayerUpgradeSystem upgradeSystem;

        [SerializeField] private GameObject knifePrefab;
        [SerializeField] private Transform knifePoint;

        [Header("Attack Settings")]
        [SerializeField] private float attackCooldown = 0.2f;

        private bool isAttacking;
        private bool attackOnCooldown;

        private void Awake()
        {
            playerInput =
                GetComponent<PlayerInput>();

            PlayerMovement =
                GetComponent<PlayerMovement>();

            // Input System haritasından
            // Move ve Jump aksiyonlarını yakala.
            moveAction =
                playerInput.actions["Move"];

            jumpAction =
                playerInput.actions["Jump"];

            // Animator Player'ın altındaki
            // Visual objesinde olduğu için
            // child objeler içinde buluyoruz.
            animator =
                GetComponentInChildren<Animator>();

            // Player'ın altındaki AttackHitbox'ı bul.
            attackHitbox =
                GetComponentInChildren<AttackHitbox>();

            playerAudio =
                GetComponent<PlayerAudio>();

            upgradeSystem =
                GetComponent<PlayerUpgradeSystem>();
        }

        private void Update()
        {
            // Move aksiyonundaki yön bilgisini oku.
            Vector2 moveInput =
                moveAction.ReadValue<Vector2>();

            // Hareket bilgisini PlayerMovement'a gönder.
            PlayerMovement.Move(moveInput);

            // Animator'a yatay hareket bilgisini gönder.
            animator.SetFloat(
                "Speed",
                Mathf.Abs(moveInput.x)
            );

            // Animator'a yerde olup olmadığımızı gönder.
            animator.SetBool(
                "IsGrounded",
                PlayerMovement.IsGrounded
            );

            // Jump aksiyonu tetiklendiyse zıpla.
            if (jumpAction.triggered)
            {
                PlayerMovement.Jump();

                if (PlayerMovement.IsGrounded == false)
                {
                    playerAudio.PlayJump();
                }
            }

            // F tuşuna basınca kılıçla saldır.
            // Saldırı devam etmiyorsa ve cooldown bitmişse saldır.
            if (Keyboard.current.fKey.wasPressedThisFrame &&
                !isAttacking &&
                !attackOnCooldown)
            {
                StartCoroutine(Attack());
            }

            // G tuşuna basınca bıçak fırlat.
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                ThrowKnife();
            }
        }

        private IEnumerator Attack()
        {
            isAttacking = true;

            // -----------------------------------------
            // SALDIRI BAŞLANGICI
            // -----------------------------------------

            // Attack animasyonunu başlat.
            animator.SetTrigger("Attack");

            // Saldırı sesi.
            playerAudio.PlayAttack();

            // Saldırının vuruş anına kadar bekle.
            yield return new WaitForSeconds(0.2f);

            // -----------------------------------------
            // HITBOX
            // -----------------------------------------

            // Hasar alanını aç.
            attackHitbox.EnableHitbox();

            // Hitbox'ın aktif kalacağı süre.
            yield return new WaitForSeconds(0.2f);

            // Hasar alanını kapat.
            attackHitbox.DisableHitbox();

            isAttacking = false;

            // -----------------------------------------
            // SALDIRI COOLDOWN
            // -----------------------------------------

            // Bir sonraki saldırıdan önce
            // 0.2 saniye bekle.
            attackOnCooldown = true;

            yield return new WaitForSeconds(
                attackCooldown
            );

            attackOnCooldown = false;
        }

        private void ThrowKnife()
        {
            if (upgradeSystem == null)
                return;

            // Envanterde bıçak yoksa fırlatma.
            if (!upgradeSystem.UseKnife())
                return;

            if (knifePrefab == null)
                return;

            if (knifePoint == null)
                return;

            // Oyuncunun baktığı yöne göre
            // bıçağın fırlatma yönünü belirle.
            float direction =
                transform.localScale.x > 0
                    ? 1f
                    : -1f;

            // Bıçağı KnifePoint konumunda oluştur.
            GameObject knife =
                Instantiate(
                    knifePrefab,
                    knifePoint.position,
                    Quaternion.identity
                );

            Vector2 throwDirection =
                new Vector2(
                    direction,
                    0f
                );

            KnifeProjectile projectile =
                knife.GetComponent<KnifeProjectile>();

            if (projectile != null)
            {
                // Bıçağa yönü,
                // upgrade sistemini
                // ve sahibi olan Player'ı gönder.
                projectile.Launch(
                    throwDirection,
                    upgradeSystem,
                    gameObject
                );
            }
        }
    }
}