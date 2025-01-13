using System.Collections;
using UnityEngine;

public class MoaiCallMethodsInAnimation : MonoBehaviour
{
    [SerializeField] private AttackPatterns attackPattern;
    [SerializeField] private Collider damageCollider;
    [SerializeField] private TriggerCameraShake cameraShake;
    [SerializeField] private MoaiBoss dropMaterials;
    [SerializeField] private GameObject moaiObject;
    public void HandStomp()
    {
        cameraShake.TriggerShakeCamera();
        attackPattern.HandStompCalledInAnimation();
    }

    public void FastHandStomp()
    {
        cameraShake.TriggerShakeCamera();
        attackPattern.FastStomp();
    }

    public void FlyAndStomp()
    {
        cameraShake.TriggerShakeCamera();
        attackPattern.FlyAndStomp();

    }

    public void DropMaterials()
    {
        dropMaterials.DropMaterials();

        StartCoroutine(BossScaledToDeath());
    }

    private IEnumerator BossScaledToDeath()
    {

        yield return new WaitForSeconds(10f);

        float lerp = 0f;
        Vector3 currentScale = moaiObject.transform.localScale;

        while(lerp < 1f)
        {
            moaiObject.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, lerp);
            lerp += Time.deltaTime;
            yield return null;
        }

        Destroy(moaiObject);
    }


    public void BecomeImmortal() => damageCollider.enabled = (false);

    public void NoLongerImmortal() => damageCollider.enabled = (true);
}
