using UnityEngine;

public class GameMenuScript : MonoBehaviour
    {
    [SerializeField] GameObject MAP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
        {
        MAP.SetActive(false);
        }

    // Update is called once per frame
    void Update()
        {

        //エンターキーが入力された場合「true」
        if (Input.GetKey(KeyCode.H))
            {
            MAP.SetActive(true);

            }
        if (Input.GetKey(KeyCode.J))
            {
            MAP.SetActive(false);

            }

        }
    }
