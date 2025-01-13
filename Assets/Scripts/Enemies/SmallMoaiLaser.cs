using UnityEngine;
using System.Collections;
public class SmallMoaiLaser : MonoBehaviour
{
    [SerializeField] private GameObject firePointOnHead;
    private GameObject player;

    private Animator animator;

    [Header("Detection"), SerializeField] private float detectionRadius = 20f;

    [Header("LaserAttack"), SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserAngle = 90f; // Total angle to rotate during the laser (in degrees)
    [SerializeField] private float laserDuration = 0.5f; // Duration of the laser motion (in seconds)
    [SerializeField] private float turnDuration = 0.3f; // Duration of the laser motion (in seconds)

    [SerializeField] private float attackCooldown = 5f;
    private float justAttacked;

    private bool attacking = true;
    private bool spawned = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        animator = GetComponentInChildren<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if ((Vector3.Distance(player.transform.position, transform.position) < detectionRadius) && !spawned)
        {
            spawned = true;
            animator.SetTrigger("Spawn");
            justAttacked = Time.time;
        }

        if (Vector3.Distance(player.transform.position, transform.position) < detectionRadius && Time.time - justAttacked > attackCooldown && attacking)
        {
            
            StartCoroutine(LaserAttack());
            transform.eulerAngles = new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180, 0);
        }


    }

    private IEnumerator LaserAttack()
    {
        justAttacked = Time.time;
        yield return new WaitForSeconds(turnDuration);

        float lerpValue = 0f;

        float yAngle = GetYAngleToTarget(player.transform.position);

        float randomOffset = Random.Range(-50f, 50f);

        Vector3 initialAngleLaser = new Vector3(0f, yAngle - laserAngle - randomOffset, 0f);
        Vector3 finalAngleLaser = new Vector3(0f, yAngle + laserAngle + randomOffset, 0f);

        GameObject laserTemp = Instantiate(laserPrefab, firePointOnHead.transform);

        while (lerpValue <= 1)
        {
            laserTemp.transform.eulerAngles = Vector3.Lerp(initialAngleLaser, finalAngleLaser, lerpValue);
            lerpValue += Time.deltaTime / laserDuration;
            transform.eulerAngles = laserTemp.transform.eulerAngles - new Vector3(0f, -180, 0f);// new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180, 0f);
            yield return null;
        }

        Destroy(laserTemp);


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

    public void StartAttacking() => attacking = true;

    public void StopAttacking() => attacking = false;

    private void OnDrawGizmosSelected()
    {


        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
