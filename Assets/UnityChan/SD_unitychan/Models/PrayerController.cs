using UnityEngine;

public class PrayerController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animator‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
    }

    void Update()
    {
        //float rlValue = Input.GetAxis("RL");
        bool isKeyHeld = Input.GetKey(KeyCode.F)
            || Input.GetKey("joystick button 1");
            //|| rlValue > 0.5f;

        anim.SetBool("is_prayering", isKeyHeld);
    }
}
