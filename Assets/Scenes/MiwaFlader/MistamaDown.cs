using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothSlider : MonoBehaviour
{

    [SerializeField] Slider DashGage;

    public float maxDashPoint = 5;

    float dashPoint;
    float currentVelocity = 0;

    void Start()
    {
        dashPoint = maxDashPoint;
        DashGage.maxValue = maxDashPoint;
        DashGage.value = maxDashPoint;
    }


    void Update()
    {
        float currentDashPT = Mathf.SmoothDamp(DashGage.value, dashPoint, ref currentVelocity, 10 * Time.deltaTime);
        DashGage.value = currentDashPT;

        if (Input.GetKeyDown(KeyCode.Space) && dashPoint < maxDashPoint)
        {
            dashPoint++;
        }

        else if (Input.GetKeyDown(KeyCode.Z) && dashPoint > 0)
        {
            dashPoint--;
        }

    }
}