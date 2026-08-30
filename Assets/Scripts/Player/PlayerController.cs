using System.Collections;
using Assets.Scripts.Projectiles;
using SolarOdyssey.Combat;
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

        [Header("Knife Settings")]
        [SerializeField] private float knifeScale = 2f;

        [Header("Attack Settings")]
        [SerializeField] private float attackCooldown = 0.2f;

        private bool isAttacking;
        private bool attackOnCooldown;

        // Oyuncunun en son baktığı/hareket ettiği yön.
        // 1 = sağ
        // -1 = sol
        private float facingDirection = 1f;

        private void Awake()
        {
            playerInput =
                GetComponent<PlayerInput>();

            PlayerMovement =
                GetComponent<PlayerMovement>();

            moveAction =
                playerInput.actions["Move"];

            jumpAction =
                playerInput.actions["Jump"];

            animator =
                GetComponentInChildren<Animator>();

            attackHitbox =
                GetComponentInChildren<AttackHitbox>();

            playerAudio =
                GetComponent<PlayerAudio>();

            upgradeSystem =
                GetComponent<PlayerUpgradeSystem>();
        }

        private void Update()
        {
            Vector2 moveInput =
                moveAction.ReadValue<Vector2>();

            PlayerMovement.Move(moveInput);

            animator.SetFloat(
                "Speed",
                Mathf.Abs(moveInput.x)
            );

            animator.SetBool(
                "IsGrounded",
                PlayerMovement.IsGrounded
            );

            // =========================================
            // OYUNCUNUN BAKTIĞI YÖNÜ KAYDET
            // =========================================

            if (moveInput.x > 0.01f)
            {
                facingDirection = 1f;
            }
            else if (moveInput.x < -0.01f)
            {
                facingDirection = -1f;
            }

            // =========================================
            // JUMP
            // =========================================

            if (jumpAction.triggered)
            {
                PlayerMovement.Jump();

                if (PlayerMovement.IsGrounded == false)
                {
                    playerAudio.PlayJump();
                }
            }

            // =========================================
            // KILIÇ SALDIRISI
            // =========================================

            if (Keyboard.current.fKey.wasPressedThisFrame &&
                !isAttacking &&
                !attackOnCooldown)
            {
                StartCoroutine(Attack());
            }

            // =========================================
            // BIÇAK
            // =========================================

            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                ThrowKnife();
            }
        }

        private IEnumerator Attack()
        {
            isAttacking = true;

            animator.SetTrigger("Attack");

            playerAudio.PlayAttack();

            yield return new WaitForSeconds(0.2f);

            attackHitbox.EnableHitbox();

            yield return new WaitForSeconds(0.2f);

            attackHitbox.DisableHitbox();

            isAttacking = false;

            attackOnCooldown = true;

            yield return new WaitForSeconds(
                attackCooldown
            );

            attackOnCooldown = false;
        }

        private void ThrowKnife()
        {
            Debug.Log("========== BIÇAK TEST ==========");

            if (upgradeSystem == null)
            {
                Debug.LogError(
                    "Knife: UpgradeSystem NULL!"
                );

                return;
            }

            if (!upgradeSystem.UseKnife())
            {
                Debug.LogWarning(
                    "Knife: Bıçak hakkı yok."
                );

                return;
            }

            if (knifePrefab == null)
            {
                Debug.LogError(
                    "Knife: Knife Prefab NULL!"
                );

                return;
            }

            if (knifePoint == null)
            {
                Debug.LogError(
                    "Knife: Knife Point NULL!"
                );

                return;
            }

            // =========================================
            // FIRLATMA YÖNÜ
            // =========================================

            Vector2 throwDirection =
                new Vector2(
                    facingDirection,
                    0f
                );

            Debug.Log(
                "Knife: Oyuncunun son yönü = " +
                facingDirection
            );

            // =========================================
            // BIÇAĞI OLUŞTUR
            // =========================================

            GameObject knife =
                Instantiate(
                    knifePrefab,
                    knifePoint.position,
                    Quaternion.identity
                );

            // =========================================
            // BIÇAK BOYUTU
            // =========================================

            knife.transform.localScale =
                new Vector3(
                    knifeScale,
                    knifeScale,
                    1f
                );

            Debug.Log(
                "Knife: Oluşturuldu -> " +
                knife.name +
                " | Scale = " +
                knife.transform.localScale
            );

            // =========================================
            // PROJECTILE SCRIPT
            // =========================================

            KnifeProjectile projectile =
                knife.GetComponentInChildren<KnifeProjectile>(
                    true
                );

            if (projectile == null)
            {
                Debug.LogError(
                    "Knife: KnifeProjectile bulunamadı!"
                );

                Destroy(knife);

                return;
            }

            // =========================================
            // FIRLAT
            // =========================================

            projectile.Launch(
                throwDirection,
                upgradeSystem,
                gameObject
            );

            Debug.Log(
                "Knife: Launch tamamlandı! " +
                "Yön = " +
                throwDirection
            );

            Debug.Log(
                "========== BIÇAK TEST BİTTİ =========="
            );
        }
    }
}