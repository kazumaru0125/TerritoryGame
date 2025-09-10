using UnityEngine;

public class CharacterMovePhysics : MonoBehaviour
{
    public float normalSpeed = 5f;        // 通常速度
    public float slowSpeed = 2f;          // 障害物接触時の減速速度
    private float currentSpeed;
    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        // 入力受け取り（例）
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
        if (collision.gameObject.CompareTag("Obstacle"))  // 障害物には"Obstacle"タグを設定
        {
            currentSpeed = slowSpeed;
            Debug.Log("障害物に接触したため速度を減速");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentSpeed = normalSpeed;
            Debug.Log("障害物から離れたので速度を戻す");
        }
    }
}
