using UnityEngine;
using System.Collections;

public class AttackPatterns : MonoBehaviour
{

    [SerializeField] private GameObject firePointOnHead;
    [SerializeField] private GameObject player;

    [SerializeField] private FollowTarget cameraLogic;
    [SerializeField] private float shakeDuration = 0.3f;

    [Header("Start"), SerializeField] private float speedStomp = 20f;

    [Header("AOEAttack"), SerializeField] private float numberOfProjectiles = 5;
    [SerializeField] private float shotSpeed = 10f;
    [SerializeField] private float attackCooldown = 5f; // Duration of the arc movement
    [SerializeField] private GameObject enemyShotPrefab;
    [SerializeField] private float radius = 10f;     // Radius of the circle
    [SerializeField] private float height = 5f; // Peak height of the arc
    [SerializeField] private float duration = 2f; // Duration of the arc movement

    [Header("LaserAttack"), SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserAngle = 90f; // Total angle to rotate during the laser (in degrees)
    [SerializeField] private float laserDuration = 0.5f; // Duration of the laser motion (in seconds)
    [SerializeField] private float turnDuration = 0.3f; // Duration of the laser motion (in seconds)


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        StartCoroutine(StartStomp());
        yield return new WaitForSeconds(2f);
        StartRandomAttack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartStomp()
    {
        while(transform.position.y > 1)
        {
            transform.position -= Vector3.up * Time.deltaTime * speedStomp;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    private void StartRandomAttack()
    {
        int rng = Random.Range(0, 4);
        if (rng == 3)
            StartCoroutine(LaserAttack());
        else
            StartCoroutine(MoveAlongArc());
    }

    private IEnumerator LaserAttack()
    {
        //float yAngle = GetYAngleToTarget(player.transform.position);

        //Vector3 initialAngleMoai = transform.eulerAngles;
        //Vector3 finalAngleMoai = new Vector3(0f, yAngle - 180 - 90, 0);


        //Debug.Log("initial: " + initialAngleMoai + ", finalAngleMoai" + finalAngleMoai);

        //float timer = 0f;

        //while (timer <= turnDuration)
        //{
        //    transform.eulerAngles = new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180 - 90, 0);
        //    timer += Time.deltaTime;
        //    yield return null;
        //}

        yield return new WaitForSeconds(turnDuration);

        float lerpValue = 0f;

        float yAngle = GetYAngleToTarget(player.transform.position);

        Vector3 initialAngleLaser = new Vector3(0f, yAngle - laserAngle, 0f);
        Vector3 finalAngleLaser = new Vector3(0f, yAngle + laserAngle, 0f);

        GameObject laserTemp = Instantiate(laserPrefab, firePointOnHead.transform);

        while (lerpValue <= 1)
        {
            laserTemp.transform.eulerAngles = Vector3.Lerp(initialAngleLaser, finalAngleLaser, lerpValue);
            lerpValue += Time.deltaTime / laserDuration;
            transform.eulerAngles = laserTemp.transform.eulerAngles - new Vector3(0f, -180, 0f);// new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180, 0f);
            yield return null;
        }

        Destroy(laserTemp);

        yield return new WaitForSeconds(attackCooldown);

        StartRandomAttack();
    }

    private float GetYAngleToTarget(Vector3 targetPosition)
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = targetPosition - transform.position;

        // Remove the y component for horizontal angle
        directionToTarget.y = 0;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        return angle;
    }


    // Call this method to get a random position within the circle
    public Vector3 GetRandomPosition()  
    {
        // Generate a random angle in radians
        float randomAngle = Random.Range(0f, Mathf.PI * 2);

        // Generate a random distance within the radius
        float randomDistance = Random.Range(15f, radius);

        // Calculate the random position around the center point
        float xOffset = Mathf.Cos(randomAngle) * randomDistance;
        float zOffset = Mathf.Sin(randomAngle) * randomDistance;

        // Return the new position
        Vector3 randomPosition = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
        return randomPosition;
    }

    public void AoeAttackOnHead()
    {

        float degreeIteration = 360 / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject shotTemp = Instantiate(enemyShotPrefab);
            shotTemp.transform.position = firePointOnHead.transform.position;
            shotTemp.transform.eulerAngles = new Vector3(0, i * degreeIteration, 0);
            shotTemp.GetComponent<Rigidbody>().linearVelocity = shotTemp.transform.forward * shotSpeed;
        }
    }

    private IEnumerator MoveAlongArc()
    {
        float timeElapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = player.transform.position;

        // Perform the arc movement over the duration
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / duration;

            // Interpolate the horizontal position (X and Z) using Lerp
            Vector3 currentHorizontalPosition = Vector3.Lerp(startPosition, endPosition, progress);

            // Calculate the height based on a parabolic trajectory (Y axis)
            float arcHeight = Mathf.Sin(Mathf.PI * progress) * height;

            // Combine the horizontal movement with the arc height
            Vector3 currentPosition = new Vector3(currentHorizontalPosition.x, startPosition.y + arcHeight, currentHorizontalPosition.z);

            // Update the object's position
            transform.position = currentPosition;

            transform.eulerAngles = new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180, 0);

            // Yield until the next frame
            yield return null;
        }

        // Ensure the object ends at the exact target position
        transform.position = endPosition;
        cameraLogic.ShakeCamera(shakeDuration, 0.2f, false);

        AoeAttackOnHead();

        yield return new WaitForSeconds(attackCooldown);

        StartRandomAttack();
    }


}
