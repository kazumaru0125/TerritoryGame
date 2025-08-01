using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
        {
        // ESCキーが押されたらアプリを終了
        if (Input.GetKeyDown(KeyCode.Escape))
            {
            Application.Quit();
            Debug.Log("ゲーム終了");
            }
        }
}
