using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting.Antlr3.Runtime.Tree;


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
    [SerializeField] private float laserAngleDefault = 90f; // Total angle to rotate during the laser (in degrees)
    [SerializeField] private float laserDurationDefault = 0.5f; // Duration of the laser motion (in seconds)
    [SerializeField] private float turnDuration = 0.3f; // Duration of the laser motion (in seconds)

    [Header("ChasePlayer"), SerializeField] private float walkMovSpeedPhase1 = 3f;
    [SerializeField] private float walkMovSpeedPhase2 = 6f;
    [SerializeField] private float flyMovSpeed = 5f;
    [SerializeField] private float flyMovSpeed2 = 10f;

    [Header("Stomp"), SerializeField] private GameObject moaiPhysicalCollider;
    [SerializeField] private Renderer[] meshRenderers;


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
    public int CurrentPhase => currentPhase;

    public void ChangePhase(int phase) => currentPhase = phase;

    private int lastAttack = 7;

    private List<GameObject> projectiles = new List<GameObject>();

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
        currentPhase = 1;

        ClearProjectiles();

        animator.SetTrigger("Nothing");
    }

    public void ClearProjectiles()
    {
        foreach (GameObject go in projectiles)
        {
            Destroy(go);
        }

        projectiles.Clear();
    }


    private void Attacks()
    {
        switch (currentPhase)
        {
            case 1:
                Phase1Attacks();
                break;
            case 2:
                Phase2Attacks();
                break;
            case 3:
                Phase3Attacks();
                break;
            case 4:
                EnragePhase();
                break;
            default:
                Debug.Log("Wrong stomp attack");
                break;
        }
    }

    private void Phase1Attacks()
    {
        int attack = Random.Range(0, 4);

        // if last attack was walking, we hand stomp
        if (lastAttack == 0)
        {
            attack = 3;
        }
        

        if(attack == lastAttack)
            attack = Random.Range(0, 4);

        Debug.Log($"Phase 1 attack {attack} chosen");

        switch (attack)
        {
            case 0:
                // Walk to player
                lastCoroutine = StartCoroutine(ChasePlayer(walkMovSpeedPhase1, 2f,  false));
                break;
            case 1:
                // Fly and stomp
                lastCoroutine = StartCoroutine(FlyAndStompPhases(1));
                break;
            case 2:
                // Laser Attack
                lastCoroutine = StartCoroutine(LaserAttack(120f, 2f));
                break;
            case 3:
                animator.SetTrigger("HandStomp");
                break;
            case 4:
                break;
            default:
                Debug.Log("Bad phase 1 attacks");
                break;
        }

        lastAttack = attack;
    }

    private void Phase2Attacks()
    {
        int attack = Random.Range(0, 4);

        // if last attack was walking, we hand stomp
        if (lastAttack == 0)
        {
            attack = 3;
        }


        if (attack == lastAttack)
            attack = Random.Range(0, 4);

        Debug.Log($"Phase 2 attack {attack} chosen");

        switch (attack)
        {
            case 0:
                // Walk to player
                lastCoroutine = StartCoroutine(ChasePlayer(walkMovSpeedPhase2, 3f, false));
                break;
            case 1:
                // Fly and stomp
                lastCoroutine = StartCoroutine(FlyAndStompPhases(2));
                break;
            case 2:
                // Laser Attack
                lastCoroutine = StartCoroutine(LaserAttack(60f, 2f));
                break;
            case 3:
                animator.SetTrigger("HandStomp");
                break;
            case 4:
                break;
            default:
                Debug.Log("Bad phase 1 attacks");
                break;
        }

        lastAttack = attack;
    }

    private void Phase3Attacks()
    {
                int attack = Random.Range(0, 4);

        // if last attack was walking, we hand stomp
        if (lastAttack == 0)
        {
            attack = 3;
        }
        

        if(attack == lastAttack)
            attack = Random.Range(0, 4);

        Debug.Log($"Phase 3 attack {attack} chosen");

        switch (attack)
        {
            case 0:
                // Walk to player
                lastCoroutine = StartCoroutine(ChasePlayer( walkMovSpeedPhase2, 2f, false));
                break;
            case 1:
                // Fly and stomp
                lastCoroutine = StartCoroutine(FlyAndStompPhases(3));
                break;
            case 2:
                // Laser Attack
                lastCoroutine = StartCoroutine(LaserAttack(60f, 2f));
                break;
            case 3:
                animator.SetTrigger("HandStomp");
                break;
            case 4:
                break;
            default:
                Debug.Log("Bad phase 1 attacks");
                break;
        }

        lastAttack = attack;
    }

    private void EnragePhase()
    {
        //        int attack = Random.Range(0, 4);

        //// if last attack was walking, we hand stomp
        //if (lastAttack == 0)
        //{
        //    attack = 3;
        //}


        //if(attack == lastAttack)
        //    attack = Random.Range(0, 4);

        //Debug.Log($"Phase Enrage attack {attack} chosen");

        //switch (attack)
        //{
        //    case 0:
        //        // Walk to player
        //        lastCoroutine = StartCoroutine(ChasePlayer(walkMovSpeedPhase2, 2f, false));
        //        break;
        //    case 1:
        //        // Fly and stomp
        //        lastCoroutine = StartCoroutine(FlyAndStompPhases(4));
        //        break;
        //    case 2:
        //        // Laser Attack
        //        lastCoroutine = StartCoroutine(LaserAttack(60f, 2f));
        //        break;
        //    case 3:
        //        animator.SetTrigger("HandStomp");
        //        break;
        //    case 4:
        //        break;
        //    default:
        //        Debug.Log("Bad phase 1 attacks");
        //        break;
        //}

        //lastAttack = attack;
        animator.SetTrigger("HandStomp Faster");
    }

    private IEnumerator FlyAndStompPhases(int phase)
    {
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(2f);
        foreach (Renderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }
        //moaiPhysicalCollider.SetActive(false);
        lastCoroutine = StartCoroutine(ChasePlayer(flyMovSpeed * phase / 3, 3f, true));
        target.SetActive(true);
        StartCoroutine(ScaleTargetDecal(3f));
        yield return new WaitForSeconds(3f);
        foreach (Renderer mesh in meshRenderers)
        {
            mesh.enabled = true;
        }
        //moaiPhysicalCollider.SetActive(true);
        target.SetActive(false);
        animator.SetTrigger("Stomp");
        // Triger attack on stomp animation
    }



    private IEnumerator ScaleTargetDecal(float time)
    {
        float lerpValue = 0f;

        while (lerpValue < 1)
        {

            lerpValue += Time.deltaTime / (time - 1.5f);
            target.transform.localScale = Vector3.Lerp(new Vector3(0.001f, 0.001f, 2f), new Vector3(0.007f, 0.007f, 2f), lerpValue);
            yield return null;
        }

        StartCoroutine(BlinkCoroutineTarget());
    }

    private IEnumerator BlinkCoroutineTarget()
    {

        float timeElapsed = 0f;

        DecalProjector targetDecal = target.GetComponent<DecalProjector>();
        Color originalColor = targetDecal.material.GetColor("_Color");

        while (timeElapsed < 1.5f)
        {
            // Calculate blink interval based on how much time is left, to increase frequency near the end
            float progress = timeElapsed / 1.5f;
            float currentBlinkInterval = Mathf.Lerp(0.3f, 0.05f, progress); // Starts slower, ends faster

            // Switch to white material
            targetDecal.material.SetColor("_Color", Color.white);
            Debug.Log(targetDecal.material.GetColor("_Color"));
            yield return new WaitForSeconds(currentBlinkInterval / 2);

            // Switch back to the original material
            targetDecal.material.SetColor("_Color", originalColor);
            Debug.Log(targetDecal.material.GetColor("_Color"));
            yield return new WaitForSeconds(currentBlinkInterval / 2);

            // Update the elapsed time
            timeElapsed += currentBlinkInterval;
        }

        // Ensure the original material is set after blinking finishes
        targetDecal.material.SetColor("_Color", originalColor);
        Debug.Log(targetDecal.material.GetColor("_Color"));
    }


    public void FlyAndStomp()
    {
        switch (currentPhase)
        {
            case 1:
                AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 8);
                break;
            case 2:
                AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 16, true, enemyShotPrefabExploding);
                break;
            case 3:
                lastCoroutine = StartCoroutine(FlyAndStomp4Waves());
                break;
            case 4:
                lastCoroutine = StartCoroutine(FlyAndStomp4Waves());
                break;
            default:
                Debug.Log("Wrong stomp attack");
                break;
        }

        Attacks();
    }

    private IEnumerator FlyAndStomp4Waves()
    {
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 5, initialAngle:18);
        yield return new WaitForSeconds(0.1f);
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefabExploding, 5, initialAngle: 18 * 2);
        yield return new WaitForSeconds(0.1f);
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefab, 5, initialAngle: 18 * 3);
        yield return new WaitForSeconds(0.1f);
        AoeAttackOnHead(firePointOnHead.transform, enemyShotPrefabExploding, 5, initialAngle: 18 * 4);

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


        Attacks();
        //lastCoroutine = StartCoroutine(LaserAttack(360f, 2f));

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

        if(!flying)
            Attacks();
        //HandStomp();
    }

    private void HandStomp()
    {
        animator.SetTrigger("HandStomp");
    }

    public void HandStompCalledInAnimation()
    {
        switch (currentPhase)
        {
            case 1:
                AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 4);
                AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 4, initialAngle: 45f);
                Attacks();
                break;
            case 2:
                bool doubleStomp = Random.Range(0, 3) < 1 ? true : false;
                if (doubleStomp)
                {
                    AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 6);
                    AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 6, initialAngle: 45f);
                    animator.SetTrigger("HandStomp Faster");
                }
                else
                {
                    AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefabExploding, 6, true, enemyShotPrefab);
                    AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 6, true, enemyShotPrefabExploding, 45f);
                    Attacks();
                }
                break;
            case 3:
                doubleStomp = Random.Range(0, 3) < 1 ? true : false;
                if (doubleStomp)
                {
                    AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 8);
                    AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 8);
                    animator.SetTrigger("HandStomp Faster");
                }
                else
                {
                    AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefabExploding, 8);
                    AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefabExploding, 8, initialAngle: 45f);
                    Attacks();
                }

                break;
            case 4:
                doubleStomp = Random.Range(0, 3) < 1 ? true : false;
                if (doubleStomp)
                {
                    AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 8);
                    AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 8);
                    animator.SetTrigger("HandStomp Faster");
                }
                else
                {
                    AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefabExploding, 8);
                    AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefabExploding, 8, initialAngle: 45f);
                    Attacks();
                }

                break;
            default:
                Debug.Log("Wrong stomp attack");
                break;
        }

        

    }

    public void FastStomp()
    {
        if(currentPhase == 2)
        {
            AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 6, initialAngle: 45f);
            AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefab, 6);
        }
        else if (currentPhase == 3)
        {
            AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 8, true, enemyShotPrefabExploding, initialAngle: 45f);
            AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefabExploding, 8, true, enemyShotPrefab);
        }
        else
        {
            AoeAttackOnHead(firePointOnLeftHand.transform, enemyShotPrefab, 5, true, enemyShotPrefabExploding, initialAngle: Random.Range(0f, 45f));
            AoeAttackOnHead(firePointOnRightHand.transform, enemyShotPrefabExploding, 4, true, enemyShotPrefab, initialAngle: Random.Range(0f, 45f));
        }

        Attacks();
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
            lastCoroutine = StartCoroutine(LaserAttack(laserAngleDefault, laserDurationDefault));
        else
            lastCoroutine = StartCoroutine(MoveAlongArc());
    }

    private IEnumerator LaserAttack(float laserAngle, float laserDuration)
    {

        animator.SetTrigger("Beam");

        yield return new WaitForSeconds(turnDuration);

        float lerpValue = 0f;

        float yAngle = GetYAngleToTarget(player.transform.position);

        Vector3 initialAngleLaser;
        Vector3 finalAngleLaser;
        if (laserAngle < 90f)
        {
            initialAngleLaser = new Vector3(0f, yAngle - laserAngle, 0f);
            finalAngleLaser = new Vector3(0f, yAngle + laserAngle, 0f);
        }
        else
        {
            initialAngleLaser = new Vector3(0f, yAngle - 90f, 0f);
            finalAngleLaser = new Vector3(0f, yAngle + laserAngle - 90f, 0f);
        }


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

        Attacks();
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

    public void AoeAttackOnHead(Transform position, GameObject projectile, int numberOfProjectiles, bool halfProjectiles = false, GameObject secondProjectile = null, float initialAngle = 0f)
    {

        float degreeIteration = 360 / numberOfProjectiles;

        if (halfProjectiles)
        {
            bool half = false;
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                GameObject shotTemp;

                if (half)
                    shotTemp = Instantiate(secondProjectile);
                else
                    shotTemp = Instantiate(projectile);

                projectiles.Add(shotTemp);

                shotTemp.transform.position = position.position;
                shotTemp.transform.eulerAngles = new Vector3(0, initialAngle + i * degreeIteration, 0);
                shotTemp.GetComponent<Rigidbody>().linearVelocity = shotTemp.transform.forward * shotSpeed;

                half = !half;
            }
        }
        else
        {
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                GameObject shotTemp = Instantiate(projectile);
                projectiles.Add(shotTemp);
                shotTemp.transform.position = position.position;
                shotTemp.transform.eulerAngles = new Vector3(0, initialAngle + i * degreeIteration, 0);
                shotTemp.GetComponent<Rigidbody>().linearVelocity = shotTemp.transform.forward * shotSpeed;
            }
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