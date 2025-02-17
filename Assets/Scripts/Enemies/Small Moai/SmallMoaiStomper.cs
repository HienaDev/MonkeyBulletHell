using UnityEngine;
using System.Collections;
public class SmallMoaiStomper : MonoBehaviour
{
    [SerializeField] private GameObject firePointOnHead;
    private GameObject player;
    private Animator animator;

    [Header("Detection"), SerializeField] private float detectionRadius = 20f;
    [Header("AOEAttack"), SerializeField] private float numberOfProjectiles = 5;
    [SerializeField] private float shotSpeed = 10f;
    [SerializeField] private float attackCooldown = 5f; // Duration of the arc movement
    [SerializeField] private GameObject enemyShotPrefab;
    [SerializeField] private float radius = 10f;     // Radius of the circle
    [SerializeField] private float height = 5f; // Peak height of the arc
    [SerializeField] private float duration = 2f; // Duration of the arc movement

    private bool attacking = false; 

    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        animator = GetComponentInChildren<Animator>();    
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < detectionRadius && !attacking)
        {
            animator.SetTrigger("Spawn");
            transform.eulerAngles = new Vector3(0f, GetYAngleToTarget(player.transform.position) - 180, 0);
            attacking = true;
        }
    }

    public void StartAttacking()
    {
        StartCoroutine(MoveAlongArc());
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

    private IEnumerator MoveAlongArc()
    {
        float timeElapsed = 0f;

        Vector3 startPosition = transform.position;

        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.transform.position - startPosition).normalized;

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(startPosition, player.transform.position);

        Vector3 endPosition = Vector3.zero;

        if (distanceToPlayer < radius)
            endPosition = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        else
        {
            endPosition = new Vector3((transform.position + directionToPlayer * radius).x, 0f, (transform.position + directionToPlayer * radius).z);;
        }

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
            
        AoeAttackOnHead();

        float newCooldown = Random.Range(attackCooldown - 1f, attackCooldown + 1f);
        yield return new WaitForSeconds(newCooldown);

        //StartRandomAttack();
        StartCoroutine(MoveAlongArc());
    }

    public void AoeAttackOnHead()
    {
        float degreeIteration = 360 / numberOfProjectiles;

        float angleToPlayer = GetYAngleToTarget(player.transform.position);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject shotTemp = Instantiate(enemyShotPrefab);
            shotTemp.transform.position = firePointOnHead.transform.position;
            shotTemp.transform.eulerAngles = new Vector3(0, i * degreeIteration + angleToPlayer, 0);
            shotTemp.GetComponent<Rigidbody>().linearVelocity = shotTemp.transform.forward * shotSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
