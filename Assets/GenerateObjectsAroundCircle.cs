using UnityEngine;

public class GenerateObjectsAroundCircle : MonoBehaviour
{
    [Header("Object Settings")]
    public GameObject objectToSpawn; // The prefab to instantiate
    public int numberOfObjects = 10; // Number of objects to generate
    public float radius = 40f; // Radius of the circle

    void Start()
    {
        GenerateObjects();
    }

    void GenerateObjects()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("No object assigned to spawn.");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate the angle for this object
            float angle = i * Mathf.PI * 2 / numberOfObjects;

            // Determine the position on the circle
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + transform.position;

            // Instantiate the object at the calculated position
            GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.identity, transform);

            // Rotate the object to face the center
            spawnedObject.transform.LookAt(transform.position);
            spawnedObject.transform.eulerAngles += new Vector3(0,-90, 0);
        }
    }

    // Optional: Visualize the circle in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + transform.position;
            Gizmos.DrawSphere(position, 0.1f);
        }
    }
}