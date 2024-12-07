using UnityEngine;
using System.Collections;
public class TriggerCameraShake : MonoBehaviour
{

    private FollowTarget cameraLogic;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private Animator animator;

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

    public void StartArenaShake()
    {
        animator.SetTrigger("ArenaStart");
    }

    public void RemoveArena()
    {
        animator.SetTrigger("ArenaClose");
    }

}
