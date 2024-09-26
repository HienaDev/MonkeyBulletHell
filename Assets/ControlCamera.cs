using TreeEditor;
using UnityEngine;

public class ControlCamera : MonoBehaviour
{

    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private KeyCode rotateLeft = KeyCode.Q;
    [SerializeField] private KeyCode rotateRight = KeyCode.E;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    private float currentZoom = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(rotateLeft))
        {
            cameraPivot.transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(rotateRight))
        {
            cameraPivot.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel") * 1000 / zoomSpeed ; //Default speed is 1000
        
        Debug.Log(currentZoom);
        if ((mouseScroll < 0 &&  currentZoom > minZoom) || (mouseScroll > 0 && currentZoom < maxZoom))
        {
            currentZoom += mouseScroll;
            cameraTransform.Translate(Vector3.forward * mouseScroll * zoomSpeed * Time.deltaTime);
        }
        
    }
}
