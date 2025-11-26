using UnityEngine;

public class CharacterMovePhysics : MonoBehaviour
{
    public float normalSpeed = 5f;        // 通常速度
    public float slowSpeed = 2f;          // 障害物接触時の減速速度
    private float currentSpeed;
    private bool isSlowing = false;       // 障害物に接触中かどうか
    private Rigidbody rb;
    private Vector3 moveInput;

    private bool canDash = true;          // ダッシュ許可フラグ

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
        canDash = true;
    }

    public bool CanDash { get { return canDash; } }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveInput = new Vector3(x, 0, z).normalized;

        // ここでダッシュ禁止の条件をつける例
        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            currentSpeed = normalSpeed * 2f; // ダッシュ時速度（例）
        }
        else if (isSlowing)
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }
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

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isSlowing = true;
            canDash = false;  // 障害物に接触中はダッシュ不可
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isSlowing = false;
            canDash = true;   // 障害物から離れたらダッシュ許可
        }
    }
}
