using UnityEngine;

public class CharacterChange : MonoBehaviour
    {
    private int index = 0;
    private int o_max = 0;
    GameObject[] childObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 変更したいモデルを事前にInspectorで割り当てておく
    public GameObject model1;
    public GameObject model2;

    void Start()
        {
        // 最初はモデル1だけを表示する
        model1.SetActive(true);
        model2.SetActive(false);
        }

    public void SwitchToModel2()
        {
        model1.SetActive(false);
        model2.SetActive(true);
        }

    public void SwitchToModel1()
        {
        model1.SetActive(true);
        model2.SetActive(false);
        }


    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyDown("z"))
            {
            SwitchToModel2();
            }

        if (Input.GetKeyDown("x"))
            {
            SwitchToModel1();
            }
        }
    }

