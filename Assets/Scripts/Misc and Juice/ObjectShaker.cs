using UnityEngine;

public class ObjectShaker : MonoBehaviour
{

    public float magnitude = 0.1f;     // Magnitude of the shake


    private Vector3 originalPosition;  // Store the initial position


    void Start()
    {
        // Save the original position of the GameObject
        originalPosition = transform.localPosition;

        // Start shaking automatically if enabled

    }

    void Update()
    {


                // Calculate a random shake offset
                Vector3 offset = Random.insideUnitSphere * magnitude;

                // Apply the offset to the GameObject's position
                transform.localPosition = originalPosition + offset;


                
            
       
        
    }

}
