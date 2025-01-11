using UnityEngine;

public class MoaiCallMethodsInAnimation : MonoBehaviour
{
    [SerializeField] private AttackPatterns attackPattern;
    [SerializeField] private Collider damageCollider;

    public void HandStomp()
    {
        attackPattern.HandStompCalledInAnimation();
    }

    public void FastHandStomp()
    {
        attackPattern.FastStomp();
    }

    public void FlyAndStomp()
    {
        attackPattern.FlyAndStomp();
    }

    public void BecomeImmortal() => damageCollider.enabled = (false);

    public void NoLongerImmortal() => damageCollider.enabled = (true);
}
