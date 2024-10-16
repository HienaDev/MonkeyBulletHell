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

    public void ShakeCameraToggle(bool toggle) => shakeCamera = toggle;
}
