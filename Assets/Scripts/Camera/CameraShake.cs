using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private float shakeAmount = 0.02f;
    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }
}
