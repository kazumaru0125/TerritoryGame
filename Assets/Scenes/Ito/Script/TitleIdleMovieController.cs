using UnityEngine;
using UnityEngine.Video;

public class TitleIdleMovieController : MonoBehaviour
{
    [Header("設定")]
    public float idleTimeToPlayMovie = 15f;
    public VideoPlayer videoPlayer;
    public AudioSource bgmSource;

    float lastInputTime;
    bool moviePlaying = false;

    // 外から状態を見るためのプロパティ
    public bool IsMoviePlaying => moviePlaying;

    // ★ このフレームでムービー側が入力を消費したかどうか
    public bool ConsumedInputThisFrame { get; private set; }

    void Start()
    {
        lastInputTime = Time.time;

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Stop();

            // 動画が最後まで再生された時のイベント登録
            videoPlayer.loopPointReached += OnMovieFinished;
        }
    }

    void Update()
    {
        // 毎フレームの頭でリセット
        ConsumedInputThisFrame = false;

        bool space = Input.GetKeyDown(KeyCode.Space);
        bool padA = Input.GetKeyDown(KeyCode.JoystickButton0);
        bool padB = Input.GetKeyDown(KeyCode.JoystickButton1);
        bool anyInput = space || padA || padB;

        if (!moviePlaying)
        {
            // ムービー非再生中：無操作時間で開始
            if (Time.time - lastInputTime >= idleTimeToPlayMovie)
            {
                PlayMovie();
            }
        }
        else
        {
            // ムービー再生中：入力があったら止めてタイトル状態へ
            if (anyInput)
            {
                StopMovie();
                lastInputTime = Time.time;
                ConsumedInputThisFrame = true;
            }
        }

        // 何かキーやボタンが押されたら、アイドルタイマーをリセット
        if (Input.anyKeyDown || anyInput)
        {
            lastInputTime = Time.time;
        }
    }

    void PlayMovie()
    {
        moviePlaying = true;

        if (bgmSource != null)
            bgmSource.mute = true;

        if (videoPlayer != null)
            videoPlayer.Play();
    }

    void StopMovie()
    {
        moviePlaying = false;

        if (videoPlayer != null)
            videoPlayer.Stop();

        // BGM を確実に元のボリュームへ
        if (bgmSource != null)
        {
            bgmSource.mute = false;
            bgmSource.volume = 1f;
        }
    }


    // 動画が最後のフレームまで再生された時に呼ばれる
    void OnMovieFinished(VideoPlayer vp)
    {
        StopMovie();               // タイトル状態に戻す（BGM復帰など）
        lastInputTime = Time.time; // ここからまた無操作タイマーを計測
    }
}
