
using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private KeyCode rotateLeft = KeyCode.Q;
    [SerializeField] private KeyCode rotateRight = KeyCode.E;
    [SerializeField] private KeyCode zoomIn = KeyCode.O;
    [SerializeField] private KeyCode zoomOut = KeyCode.P;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float zoomSpeed = 10;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    private float currentZoom = 1;
    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = transform;

        if (PlayerPrefs.HasKey("CameraSensitivity"))
        {
            rotationSpeed = PlayerPrefs.GetFloat("CameraSensitivity");
        }
        else
        {
            rotationSpeed = 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rotationSpeed = PlayerPrefs.GetFloat("CameraSensitivity", 100f);

        Vector3 dir = playerTransform.position - cameraTransform.position;

        if (Input.GetKey(rotateLeft))
        {
            cameraPivot.transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(rotateRight))
        {
            cameraPivot.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }

        if(Input.GetKeyDown(zoomIn) && currentZoom - zoomSpeed > maxZoom)
        {
            currentZoom -= zoomSpeed;
            cameraTransform.position += dir * zoomSpeed;
        }
        if(Input.GetKeyDown(zoomOut) && currentZoom + zoomSpeed < minZoom)
        {
            currentZoom += zoomSpeed;
            cameraTransform.position -= dir * zoomSpeed;
        }

        //if (cameraTransform.position.y > minZoom)
        //{
        //    cameraTransform.position -= dir * zoomSpeed;
        //}
        //if (cameraTransform.position.y < maxZoom)
        //{
        //    cameraTransform.position += dir * zoomSpeed;
        //}
    }
}
