using System.Collections;
using UnityEngine;

public class ExplodeIntoMoreShots : MonoBehaviour
{

    [SerializeField] private int numberOfShots = 4;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private float timerToExplode;
    [SerializeField] private float shotSpeed = 10f;
    [SerializeField] private float blinkSpeed = 0.2f;

    [SerializeField] private Material whiteMaterial;  

    private Renderer objectRenderer;
    private Material defaultMaterial;
    private bool isBlinking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            defaultMaterial = objectRenderer.material;
        }
        if (!isBlinking)
        {
            StartCoroutine(BlinkCoroutine());
        }
        StartCoroutine(ExplodeAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private IEnumerator ExplodeAfterTime()
    {
        float timer = 0;
        while(timer < timerToExplode)
        {
            timer += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 4, timer / timerToExplode);
            yield return null;
        }

        Explode();
    }

    private void Explode()
    {

        float degreeIteration = 360 / numberOfShots;

        for (int i = 0; i < numberOfShots; i++)
        {
            GameObject shotTemp = Instantiate(shotPrefab);
            shotTemp.transform.position = transform.position;
            shotTemp.transform.eulerAngles = new Vector3(0, i * degreeIteration, 0);
            shotTemp.GetComponent<Rigidbody>().linearVelocity = shotTemp.transform.forward * shotSpeed;
        }

        Destroy(gameObject, 0.1f);
    }

    private IEnumerator BlinkCoroutine()
    {
        isBlinking = true;

        float timeElapsed = 0f;

        while (timeElapsed < timerToExplode)
        {
            // Calculate blink interval based on how much time is left, to increase frequency near the end
            float progress = timeElapsed / timerToExplode;
            float currentBlinkInterval = Mathf.Lerp(0.3f, 0.05f, progress); // Starts slower, ends faster

            // Switch to white material
            objectRenderer.material = whiteMaterial;
            yield return new WaitForSeconds(currentBlinkInterval / 2);

            // Switch back to the original material
            objectRenderer.material = defaultMaterial;
            yield return new WaitForSeconds(currentBlinkInterval / 2);

            // Update the elapsed time
            timeElapsed += currentBlinkInterval;
        }

        // Ensure the original material is set after blinking finishes
        objectRenderer.material = defaultMaterial;
        isBlinking = false;
    }
}
