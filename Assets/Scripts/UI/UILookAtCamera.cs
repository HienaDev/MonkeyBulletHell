using UnityEngine;


public class UILookAtCamera : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
