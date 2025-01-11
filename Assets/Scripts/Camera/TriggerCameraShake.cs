using UnityEngine;
using System.Collections;
public class TriggerCameraShake : MonoBehaviour
{

    private FollowTarget cameraLogic;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject defaultPoint;
    [SerializeField] private GameObject fightingPoint;

    [SerializeField] private float goUpDuration;
    [SerializeField] private GenerateObjectsAroundCircle wallsScript;

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
