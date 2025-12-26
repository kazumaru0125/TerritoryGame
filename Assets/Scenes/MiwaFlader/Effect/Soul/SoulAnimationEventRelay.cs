using UnityEngine;

public class SoulAnimationEventRelay : MonoBehaviour
{
    public SoulAnimationSequence manager;

    // AnimationEvent ‚©‚çŒÄ‚Î‚ê‚é
    public void OnSoulAnimationFinished()
    {
        if (manager != null)
        {
            manager.OnSoulAnimationFinished();
        }
    }
}
