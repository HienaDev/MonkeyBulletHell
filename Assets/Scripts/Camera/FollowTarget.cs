using System.Collections;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private bool shakeCamera;
    [SerializeField] private float shakeAmount = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        if(target != null && transform.position != target.transform.position)
            transform.position = target.transform.position;

        if(shakeCamera)
            transform.position = transform.position + Random.insideUnitSphere * shakeAmount;
    }

    public void ShakeCamera(float duration, float intensity, bool decaying)
    {
        StartCoroutine(ShakeCameraCR(duration, intensity, decaying));
    }

    private IEnumerator ShakeCameraCR(float duration, float intensity, bool decaying)
    {
        if(!decaying)
        {
            shakeCamera = true;
            shakeAmount = intensity;
            yield return new WaitForSeconds(duration);
            shakeCamera = false;
        }
        else
        {
            shakeCamera = true;
            shakeAmount = intensity;

            float lerpValue = 0;
            while(lerpValue < 1)
            {
                lerpValue += Time.deltaTime / duration;
                shakeAmount = Mathf.Lerp(intensity, 0.2f, lerpValue);
                yield return null;
            }

            shakeCamera = false;
        }
    }
}
