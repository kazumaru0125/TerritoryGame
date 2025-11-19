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
        if (cam == null) return;

        // ƒJƒƒ‰‚Ì•ûŒü‚ğŒü‚­iY²”½“]‚µ‚È‚¢•û®j
        Vector3 targetPos = transform.position + cam.transform.rotation * Vector3.forward;
        Vector3 targetUp = cam.transform.rotation * Vector3.up;

        transform.LookAt(targetPos, targetUp);
        }
    }
