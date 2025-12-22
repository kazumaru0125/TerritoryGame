using UnityEngine;

public class DepressEffect : MonoBehaviour
{
    [SerializeField] GameObject effectRoot;

    public void Play()
    {
        effectRoot.SetActive(true);
    }
}
