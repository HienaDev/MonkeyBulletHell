using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoaiCallMethodsInAnimation : MonoBehaviour
{
    [SerializeField] private AttackPatterns attackPattern;
    [SerializeField] private Collider damageCollider;
    [SerializeField] private TriggerCameraShake cameraShake;
    [SerializeField] private MoaiBoss dropMaterials;
    [SerializeField] private GameObject moaiObject;
    [SerializeField] private GameObject returnToIslandPopUp;
    private AnimationSounds sound;

    private void Start()
    {
        sound = GetComponent<AnimationSounds>();
    }
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

        float lerp = 0f;
        float currentVolume = sound.BossMusic.volume;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / 3f;

            sound.BossMusic.volume = Mathf.Lerp(currentVolume, 0f, lerp);
            yield return null;
        }

        
        yield return new WaitForSeconds(10f);

        lerp = 0f;
        Vector3 currentScale = moaiObject.transform.localScale;

        while(lerp < 1f)
        {
            moaiObject.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, lerp);
            lerp += Time.deltaTime;
            yield return null;
        }



        yield return new WaitForSeconds(5f);
        returnToIslandPopUp.SetActive(true);

        yield return null;

        Destroy(moaiObject);
    }

    public void ResetAnimations()
    {
        GetComponent<Animator>().SetBool("Nothing", false);
    }


    public void BecomeImmortal() => damageCollider.enabled = (false);

    public void NoLongerImmortal() => damageCollider.enabled = (true);
}
