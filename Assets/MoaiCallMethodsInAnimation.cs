using UnityEngine;

public class MoaiCallMethodsInAnimation : MonoBehaviour
{
    [SerializeField] private AttackPatterns attackPattern;
    [SerializeField] private Collider damageCollider;
    [SerializeField] private TriggerCameraShake cameraShake;

    public void HandStomp()
    {
        cameraShake.TriggerShake();
        attackPattern.HandStompCalledInAnimation();
    }

    public void FastHandStomp()
    {
        cameraShake.TriggerShake();
        attackPattern.FastStomp();
    }

    public void FlyAndStomp()
    {
        cameraShake.TriggerShake();
        attackPattern.FlyAndStomp();

    }

    public void BecomeImmortal() => damageCollider.enabled = (false);

    public void NoLongerImmortal() => damageCollider.enabled = (true);
}
