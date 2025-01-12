using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate the object around its Y-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
