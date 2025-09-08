using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMap : MonoBehaviour
{
    [SerializeField]
    private TextAsset test;

    private string[] textData;
    private string[,] testMap;

    private int tateNumber; // 行数に相当
    private int yokoNumber; // 列数に相当

    [SerializeField]
    private GameObject hiwall;
    [SerializeField]
    private GameObject lowwall;
    [SerializeField]
    private GameObject nomalwall;
    [SerializeField]
    private GameObject floor;


    // Start is called before the first frame update
    void Start()
    {
        string textLines = test.text; // テキストの全体データの代入
        print(textLines);

        // 改行でデータを分割して配列に代入
        //textData = textLines.Split('\n');
        // 改行でデータを分割（空行は無視）
        textData = test.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 行数と列数の取得
        yokoNumber = textData[0].Split(',').Length;
        tateNumber = textData.Length;

        print("tate" + tateNumber);
        print("yoko" + yokoNumber);

        // ２次元配列の定義
        testMap = new string[tateNumber, yokoNumber];

        for (int i = 0; i < tateNumber; i++)
        {
            string[] tempWords = textData[i].Split(',');

            for (int j = 0; j < yokoNumber; j++)
            {
                if (j < tempWords.Length) // ← 安全チェック
                {
                    testMap[i, j] = tempWords[j];

                    if (testMap[i, j] != null)
                    {
                        switch (testMap[i, j])
                        {
                            case "1":
                                Instantiate(hiwall, new Vector3(-4.5f + j, 1.0f, 4.5f - i), Quaternion.identity);
                                //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "2":
                                Instantiate(nomalwall, new Vector3(-4.5f + j, 0.5f, 4.5f - i), Quaternion.identity);
                                //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "3":
                                Instantiate(lowwall, new Vector3(-4.5f + j, 0.25f, 4.5f - i), Quaternion.Euler(90, 0, 0));
                                //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "4":
                                Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "0":
                                Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "99":
                                Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;
                        }
                        Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                    }
                    //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                }
            }
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
