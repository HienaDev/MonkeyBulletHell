using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private float shakeAmount = 0.02f;
    private Vector3 initialPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
