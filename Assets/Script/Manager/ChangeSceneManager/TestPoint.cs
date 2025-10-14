using UnityEngine;

public class TestPoint : MonoBehaviour
{
    public int point = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
        {
        if (Input.GetKey(KeyCode.X))
            {
            point++;
            }
        if (Input.GetKey(KeyCode.Z))
            {
            point--;
            }
        }
    }
