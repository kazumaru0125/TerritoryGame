using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
{
    private Animator animator;
    private bool isRun = false;
    private Rigidbody rb;
    public float speed = 5f; // ˆÚ“®‘¬“x
    private Vector3 latestMove = Vector3.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = forward * z + right * x;
        Vector3 moveDir = Vector3.zero;

        if (move.magnitude > 0.05f)
        {
            moveDir = move.normalized * speed;  // ‘¬“xˆê’è‚Ì‚½‚ß³‹K‰»
            // ‚±‚±‚É‰ñ“]ˆ—‚à“ü‚ê‚Ä‚àOK‚Å‚·
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        latestMove = moveDir;
    }

    void FixedUpdate()
    {
        if (latestMove.magnitude > 0)
        {
            Vector3 newPos = rb.position + latestMove * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }
}
