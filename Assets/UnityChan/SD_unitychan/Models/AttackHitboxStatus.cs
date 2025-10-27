using UnityEngine;

public class AttackHitboxStatus : MonoBehaviour
{
    public bool isAttacking;

    public void StartHitbox()
    {
        isAttacking = true;
        Debug.Log("ƒƒHitbox ON!„„");
    }

    public void EndHitbox()
    {
        isAttacking = false;
        Debug.Log("ƒƒHitbox OFF!„„");
    }
}
