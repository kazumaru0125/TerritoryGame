using UnityEngine;

public class HumanSoulAnimationEventRelay : MonoBehaviour
{
    public HumanSoulAnimationSequence manager;

    // AnimationEvent ‚©‚çŒÄ‚Î‚ê‚é
    public void OnHumanSoulAnimationFinished()
    {
        if (manager != null)
        {
            manager.OnHumanSoulAnimationFinished();
        }
    }
}
