using UnityEngine;

public class BillBoard : MonoBehaviour
    {
    Camera cam;

    void Start()
        {
        cam = Camera.main;
        }

    void LateUpdate()
        {
        if (cam == null)
            {
            cam = Camera.main;
            if (cam == null) return;
            }

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f; // Å© çÇÇ≥ê¨ï™ÇêÿÇÈ

        if (camForward.sqrMagnitude < 0.001f) return;

        transform.rotation = Quaternion.LookRotation(camForward, Vector3.up);
        }

    }
