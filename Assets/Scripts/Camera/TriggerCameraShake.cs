using UnityEngine;
using System.Collections;
public class TriggerCameraShake : MonoBehaviour
{

    private FollowTarget cameraLogic;
    [SerializeField] private float shakeDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraLogic = FindFirstObjectByType<FollowTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerShake()
    {
        cameraLogic.ShakeCamera(shakeDuration, 1f, true);
    }


}
