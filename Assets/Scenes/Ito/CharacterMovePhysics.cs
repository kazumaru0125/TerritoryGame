using UnityEngine;

public class CharacterMovePhysics : MonoBehaviour
{
    public float normalSpeed = 5f;        // í èÌë¨ìx
    public float slowSpeed = 2f;          // è·äQï®ê⁄êGéûÇÃå∏ë¨ë¨ìx
    private float currentSpeed;
    private Rigidbody rb;
    private Vector3 moveInput;

    private Renderer objRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        // ì¸óÕéÛÇØéÊÇËÅió·Åj
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveInput = new Vector3(x, 0, z).normalized;
    }

    void FixedUpdate()
    {
        if (moveInput.magnitude > 0)
        {
            Vector3 moveVelocity = moveInput * currentSpeed;
            Vector3 newPosition = rb.position + moveVelocity * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentSpeed = slowSpeed;
            Debug.Log("è·äQï®Ç…ê⁄êGÇµÇΩÇÃÇ≈ë¨ìxÇå∏ë¨");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentSpeed = slowSpeed;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentSpeed = normalSpeed;
            Debug.Log("è·äQï®Ç©ÇÁó£ÇÍÇΩÇÃÇ≈ë¨ìxÇñﬂÇ∑");
        }
    }

}
