using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class AttackPatterns : MonoBehaviour
{

    [SerializeField] private GameObject firePointOnHead;
    [SerializeField] private GameObject firePointOnLeftHand;
    [SerializeField] private GameObject firePointOnRightHand;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject target;

    [SerializeField] private FollowTarget cameraLogic;
    [SerializeField] private float shakeDuration = 0.3f;

    [SerializeField] private GameObject bossHealthBar;
    private HealthSystem healthSystem;

    [Header("Start"), SerializeField] private float speedStomp = 20f;

    [Header("AOEAttack"), SerializeField] private float shotSpeed = 10f;
    [SerializeField] private float attackCooldown = 5f; // Duration of the arc movement
    [SerializeField] private GameObject enemyShotPrefabExploding;
    [SerializeField] private GameObject enemyShotPrefab;
    [SerializeField] private float radius = 10f;     // Radius of the circle
    [SerializeField] private float height = 5f; // Peak height of the arc
    [SerializeField] private float duration = 2f; // Duration of the arc movement

    [Header("LaserAttack"), SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserAngle = 90f; // Total angle to rotate during the laser (in degrees)
    [SerializeField] private float laserDuration = 0.5f; // Duration of the laser motion (in seconds)
    [SerializeField] private float turnDuration = 0.3f; // Duration of the laser motion (in seconds)

    [Header("ChasePlayer"), SerializeField] private float walkMovSpeedPhase1 = 3f;
    [SerializeField] private float walkMovSpeedPhase2 = 6f;
    [SerializeField] private float flyMovSpeed = 5f;
    [SerializeField] private float flyMovSpeed2 = 10f;

    [Header("Eyes"), SerializeField] private GameObject leftEye;
    private Material leftEyeMaterial;
    [SerializeField] private GameObject rightEye;
    private Material rightEyeMaterial;
    private Color emissionColor;
    private Color adjustedEmissionColor;

    [SerializeField] private Transform startPosition;
    private Quaternion initialRotation;

    private Coroutine lastCoroutine;

    [SerializeField] private Animator animator;

    private int currentPhase = 1;

    public void ChangePhase(int phase) => currentPhase = phase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        initialRotation = transform.rotation;

        leftEyeMaterial = leftEye.GetComponent<Renderer>().material;
        rightEyeMaterial = rightEye.GetComponent<Renderer>().material;
        // Enable the emission keyword to activate emission
        leftEyeMaterial.EnableKeyword("_EMISSION");

        // Get the current emission color
        emissionColor = leftEyeMaterial.GetColor("_EmissionColor");

        // Scale the color based on the intensity value
        adjustedEmissionColor = emissionColor * Mathf.Pow(2, 15); // Matches HDR scaling

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetImmortal(true);



    }


    public void ResetBoss()
    {
        leftEyeMaterial.SetColor("_EmissionColor", emissionColor);
        rightEyeMaterial.SetColor("_EmissionColor", emissionColor);


        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);

        transform.position = startPosition.position;
        transform.rotation = initialRotation;
        animator.SetTrigger("Nothing");
    }

    private void Phase1Attacks()
    {
        int attack = Random.Range(0, 5);

        switch(attack)
        {
            case 0:
                // Walk to player
                lastCoroutine = StartCoroutine(ChasePlayer(2f, walkMovSpeedPhase1, false));
                break;
            case 1:
                // Fly and stomp
                lastCoroutine = StartCoroutine(ChasePlayer(2f, flyMovSpeed, true));
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                Debug.Log("Bad phase 1 attacks");
                break;
        }


    }

    private IEnumerator FlyAndStompPhase1()
    {
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(2f);
        lastCoroutine = StartCoroutine(ChasePlayer(3f, flyMovSpeed, true));
        target.SetActive(true);
        StartCoroutine(ScaleTargetDecal(3f));
        yield return new WaitForSeconds(3f);
        target.SetActive(false);
        animator.SetTrigger("Stomp");
        // Triger attack on stomp animation
    }

    private IEnumerator ScaleTargetDecal(float time)
    {
        float lerpValue = 0f;

        while(lerpValue < 1)
        {

            lerpValue += Time.deltaTime / time;
            target.transform.localScale = Vector3.Lerp(new Vector3(0.001f, 0.001f, 0.001f), new Vector3(0.007f, 0.007f, 0.007f), lerpValue);
            yield return null;
        }
    }

    public void FlyAndStomp()
    {
        switch (currentPhase)
        {
            case 1:
                AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 8);
                break;
            case 2:
                AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 8);
                AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefabExploding, 8);
                break;
            case 3:
                lastCoroutine = StartCoroutine(FlyAndStomp4Waves());
                break;
            case 4:
                break;
            default:
                Debug.Log("Wrong stomp attack");
                break;
        }
    }

    private IEnumerator FlyAndStomp4Waves()
    {
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 5);
        yield return new WaitForSeconds(1f);
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefabExploding, 5);
        yield return new WaitForSeconds(1f);
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 5);
        yield return new WaitForSeconds(1f);
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefabExploding, 5);

    }

    public void StartCombat()
    {
        healthSystem.SetImmortal(false);
        bossHealthBar.SetActive(true);
        StartCoroutine(StartCombatCR());
    }

    public IEnumerator StartCombatCR()
    {
        animator.SetTrigger("Spawn");
        yield return new WaitForSeconds(5f);

        

        lastCoroutine = StartCoroutine(FlyAndStompPhase1());

        //StartRandomAttack();
    }
    private IEnumerator ChasePlayer(float movSpeed, float duration, bool flying)
    {

        float timer = 0f;

        if (!flying)
            animator.SetTrigger("Walk");

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.eulerAngles = new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180, 0);
            Vector3 dir = new Vector3(player.transform.position.x, 0f, player.transform.position.z) - new Vector3(transform.position.x, 0f, transform.position.z);
            transform.position += dir.normalized * movSpeed * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(LaserAttack());
        //HandStomp();
    }

    private void HandStomp()
    {
        animator.SetTrigger("HandStomp");
    }

    public void HandStompCalledInAnimation()
    {
        AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 5);
        AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 5);
    }

    private IEnumerator StartStomp()
    {
        while (transform.position.y > 1)
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
            lastCoroutine = StartCoroutine(LaserAttack());
        else
            lastCoroutine = StartCoroutine(MoveAlongArc());
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
        animator.SetTrigger("Beam");

        yield return new WaitForSeconds(turnDuration);

        float lerpValue = 0f;

        float yAngle = GetYAngleToTarget(player.transform.position);

        Vector3 initialAngleLaser = new Vector3(0f, yAngle - laserAngle, 0f);
        Vector3 finalAngleLaser = new Vector3(0f, yAngle + laserAngle, 0f);

        GameObject laserTemp = Instantiate(laserPrefab, firePointOnHead.transform);

        laserTemp.transform.position += new Vector3(0f, 3f, 0f);

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

    public void AoeAttackOnHead(Transform position, GameObject projectile, int numberOfProjectiles)
    {

        float degreeIteration = 360 / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject shotTemp = Instantiate(projectile);
            shotTemp.transform.position = position.position;
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

        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 5);

        yield return new WaitForSeconds(attackCooldown);

        StartRandomAttack();
    }


}