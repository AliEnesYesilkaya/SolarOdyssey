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
        public bool IsGrounded => isGrounded;
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
            if (!isGrounded) //havadaysa zıplama
                return;
            // yatay hızı koru ve zıpla
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;//zıpladığı için havada 
        }

        private void Flip(float horizontalInput)
        {// sağa sola bakma basılma durumuna göre karakteri baktır.
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(2f,2f, 2f);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-2f,2f, 2f);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))//karakter yerde ground la teması varsa
            {
                isGrounded = true;

            }
        }

        private void OnCollisionExit2D(Collision2D collision) 
        {
            if (collision.gameObject.CompareTag("Ground")) //zeminle teması kesildi ise havadadır
            {
                isGrounded = false;
            }
        }
    }
}
