using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnCallChangeFace()
    {
        // ‰½‚àˆ—‚µ‚È‚­‚ÄOK
    }

    void Update()
    {
        bool isWalking = false;
        bool isRunning = false;

        if (Input.GetKey("up"))
        {
            isWalking = true;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isRunning = true;
                transform.position += transform.forward * 0.01f; //‘–‚é‚Í•à‚­‚Ì2”{‚Ì‘¬‚³
            }
            else
            {
                transform.position += transform.forward * 0.005f;
            }
        }
        else if (Input.GetKey("down"))
        {
            isWalking = true;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isRunning = true;
                transform.position -= transform.forward * 0.01f;
            }
            else
            {
                transform.position -= transform.forward * 0.005f;
            }
        }

        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 0.2f, 0);
            // ‰EˆÚ“®’†‚Í•à‚¢‚Ä‚¢‚é‚©‘–‚Á‚Ä‚¢‚é‚©”»’è‚ÍŠÜ‚ß‚¸‰ñ“]‚É‚Ì‚İ‚µ‚Ä‚Ü‚·
        }
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -0.2f, 0);
        }

        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);
    }
}
