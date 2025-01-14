using UnityEngine;

public class StartAttackingPivot : MonoBehaviour
{
    private SmallMoaiStomper stomperScript;

    void Start()
    {
        stomperScript = GetComponentInParent<SmallMoaiStomper>();
    }

    public void StartAttack() => stomperScript.StartAttacking();
}
