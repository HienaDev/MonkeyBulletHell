using UnityEngine;

public class StartAttackingPivot : MonoBehaviour
{

    private SmallMoaiStomper stomperScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stomperScript = GetComponentInParent<SmallMoaiStomper>();
    }

 
    

    public void StartAttack() => stomperScript.StartAttacking();
}
