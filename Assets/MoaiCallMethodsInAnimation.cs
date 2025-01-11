using UnityEngine;

public class MoaiCallMethodsInAnimation : MonoBehaviour
{
    [SerializeField] private AttackPatterns attackPattern;

    public void HandStomp()
    {
        attackPattern.HandStompCalledInAnimation();
    }

    public void FlyAndStomp()
    {
        attackPattern.FlyAndStomp();
    }
}
