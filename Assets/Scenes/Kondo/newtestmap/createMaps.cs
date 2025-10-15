using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMaps : MonoBehaviour
{
    [SerializeField]
    private TextAsset test;

    private string[] textData;
    private string[,] testMap;

    private int tateNumber; // 行数に相当
    private int yokoNumber; // 列数に相当

    [SerializeField]
    private GameObject hiwalls;
    [SerializeField]
    private GameObject lowwalls;
    [SerializeField]
    private GameObject nomalwalls;
    [SerializeField]
    private GameObject floors;
    [SerializeField]
    private GameObject items;
    [SerializeField]
    private GameObject starts;
    [SerializeField]
    private GameObject ofudas;
    [SerializeField]
    private GameObject boxs;
    [SerializeField]
    private GameObject jumps;
    /*
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject start;
    */

    // 拡大倍率を指定（今回は5倍）
    private float scale = 5.0f;

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
                            case "1"://高い壁
                                Instantiate(hiwalls, new Vector3((-4.5f + j) * scale, 5.0f, (4.5f - i) * scale), Quaternion.identity);
                                //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "2"://鬼が超えれる壁
                                Instantiate(nomalwalls, new Vector3((-4.5f + j) * scale, 2.5f, (4.5f - i) * scale), Quaternion.identity);
                                //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "3"://全員が超えれる壁
                                Instantiate(lowwalls, new Vector3((-4.5f + j) * scale, 0.625f, (4.5f - i) * scale), Quaternion.Euler(90, 0, 0));
                                //Instantiate(floor, new Vector3(-4.5f + j, -0.5f, 4.5f - i), Quaternion.identity);
                                break;

                            case "4"://収集アイテム
                                Instantiate(items, new Vector3((-4.5f + j) * scale, 1.0f, (4.5f - i) * scale), Quaternion.identity);
                                break;

                            case "5"://スタート地点
                               Instantiate(starts, new Vector3((-4.5f + j) * scale, 0.0f, (4.5f - i) * scale), Quaternion.identity);
                               break;
                                
                                case "6"://お札
                                Instantiate(ofudas, new Vector3((-4.5f + j) * scale, 1.0f, (4.5f - i) * scale), Quaternion.Euler(-90f, 0f, 0f));
                                break;
                                case "7"://アイテムボックス
                                Instantiate(boxs, new Vector3((-4.5f + j) * scale, 1.0f, (4.5f - i) * scale), Quaternion.identity);
                                break;
                                case "8"://ジャンプ
                                Instantiate(jumps, new Vector3((-4.5f + j) * scale, 0.0f, (4.5f - i) * scale), Quaternion.identity);
                                break;
                                /*
                                case "9"://収集アイテム
                                Instantiate(item, new Vector3((-4.5f + j) * scale, 1.0f, (4.5f - i) * scale), Quaternion.identity);
                                break;
                                case "10"://収集アイテム
                                Instantiate(item, new Vector3((-4.5f + j) * scale, 1.0f, (4.5f - i) * scale), Quaternion.identity);
                                break;
                                */

                        }
                        Instantiate(floors, new Vector3((-4.5f + j) * scale,-2.5f, (4.5f - i) * scale), Quaternion.identity);
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
