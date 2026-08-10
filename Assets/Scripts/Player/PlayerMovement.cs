using UnityEngine;

// kodlarda çakışma olmaması için 
namespace SolarOdyssey.Player
{
    public class PlayerMovement : MonoBehaviour
    {

        private Rigidbody2D rb;
        [SerializeField] private float moveSpeed = 5f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 7f;

        private bool isGrounded = true;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {

        }
        //PlayerController sınıfından gelen yön verisine göre hareket
        public void Move(Vector2 moveInput)
        { // transform fizik kurallarını yok sayıp ışınlama yaptığı için velocity kullanıyoruz yatay hızda 
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
            Flip (moveInput.x);
        }

        public void Jump()
        {
            if (!isGrounded)
                return;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        private void Flip(float horizontalInput)
        {// sağa sola bakma basılma durumuna göre karakteri baktır.
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(0.8f,1.6f, 1f);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-0.8f,1.6f, 1f);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}
