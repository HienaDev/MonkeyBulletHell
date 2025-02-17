using UnityEngine;
using System.Collections;

public class TriggerCameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private GameObject defaultPoint;
    [SerializeField] private GameObject fightingPoint;

    [SerializeField] private float goUpDuration;
    [SerializeField] private GenerateObjectsAroundCircle wallsScript;

    private FollowTarget cameraLogic;

    void Start()
    {
        cameraLogic = FindFirstObjectByType<FollowTarget>();
    }

    public void TriggerShakeCamera()
    {
        cameraLogic.ShakeCamera(0.5f, 1f, true);
    }

    public void TriggerShake()
    {
        cameraLogic.ShakeCamera(shakeDuration, 1f, true);
        wallsScript.WallsComeUp();
        StartGoingToFightingPoint();
    }

    public void StartGoingToFightingPoint()
    {
        StartCoroutine(GoToPosition(transform.position, fightingPoint.transform.position));
    }

    public void StartGoingToDefaultPoint()
    {
        StartCoroutine(GoToPosition(transform.position, defaultPoint.transform.position));
    }

    private IEnumerator GoToPosition(Vector3 startPosition, Vector3 targetPosition)
    {
        float lerpValue = 0;
        while(lerpValue < 1f)
        {
            lerpValue += Time.deltaTime / goUpDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, lerpValue);
            yield return null;
        }
       
    }
}
