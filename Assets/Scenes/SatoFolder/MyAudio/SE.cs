using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
    {

    public AudioClip  Ok;
    public AudioClip Cancel;
    AudioSource _audioSource;

    void Start()
        {
        _audioSource = GetComponent<AudioSource>(); //Componentを取得
        }

    void Update()
        {
        KeyDownPlay();
        }

    // キーが押されたら音を鳴らす
    public void KeyDownPlay()
        {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
            _audioSource.PlayOneShot(Ok);
            }
        if (Input.GetKeyDown(KeyCode.Space))
            {
            _audioSource.PlayOneShot(Cancel);
            }
        }

    // ボタンを押したら音を鳴らす
    // ※このやり方をする場合EventTriggerは不要です。ボタンを押したらこの関数を呼び出すようにしてください
    public void PlayOk()
        {
        _audioSource.PlayOneShot(Ok);
        }

    public void PlayCancel()
        {
        _audioSource.PlayOneShot(Cancel);
        }

    }
