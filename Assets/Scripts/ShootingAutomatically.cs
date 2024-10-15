using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ShootingAutomatically : MonoBehaviour
{

    [SerializeField] private float shotSpeed = 200f;
    [SerializeField] private float fireRate = 0.5f;
    private float justShot;
    private Animator animator;

    [SerializeField] private GameObject firePoint;
    private GameObject shotManager;
    [SerializeField] private GameObject shotPrefab;
    private List<GameObject> instantiatedShots;
    private int currentShot = 0;

    private Transform target;
    private bool targetInRange;
    private List<Transform> players;

    // Start is called before the first frame update
    void Start()
    {

        justShot = Time.time;

        animator = GetComponent<Animator>();

        instantiatedShots = new List<GameObject>();

        players = new List<Transform>();

        shotManager = TAG_ShotManager_SINGLETON.Instance.gameObject;

        StartCoroutine(CreateShots());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - justShot > fireRate)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            float x = transform.rotation.eulerAngles.x;
            float z = transform.rotation.eulerAngles.z;
            Quaternion tempRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Euler(x, tempRotation.eulerAngles.y, z);
        }
    }

    private void Shoot()
    {

        target = FindClosestPlayer();

        if (target != null)
        {
            instantiatedShots[currentShot].transform.position = firePoint.transform.position;

            Vector3 direction = target.position - instantiatedShots[currentShot].transform.position;
            instantiatedShots[currentShot].transform.up = direction;

            instantiatedShots[currentShot].SetActive(true);

            instantiatedShots[currentShot].GetComponent<Rigidbody>().linearVelocity = instantiatedShots[currentShot].transform.up * shotSpeed;

            currentShot++;

            if (currentShot >= instantiatedShots.Count)
            {
                currentShot = 0;
            }

            justShot = Time.time;
            //animator.SetTrigger("Shoot");
            
        }
            
    }

    private Transform FindClosestPlayer()
    {
        Transform closestPlayer = null;
        float shortestDistance = Mathf.Infinity; // Start with the largest possible distance
        Vector3 currentPosition = transform.position; // Position of the object owning this script

        // Iterate over each target in the array
        foreach (Transform player in players)
        {

            float distanceToPlayer = Vector3.Distance(currentPosition, player.position);

            // Check if the current player is closer than the previous closest
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    public void ResetTrigger()
    {
        animator.ResetTrigger("Shoot");

    }

    private IEnumerator CreateShots()
    {
        for (int i = 0; i < 10 / fireRate; i++)
        {
            GameObject shotClone = Instantiate(shotPrefab, shotManager.transform);
            shotClone.SetActive(false);
            instantiatedShots.Add(shotClone);
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<TAG_Player>() != null) { players.Add(other.gameObject.GetComponentInChildren<TAG_Player>().gameObject.transform); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<TAG_Player>() != null) { players.Remove(other.gameObject.GetComponentInChildren<TAG_Player>().gameObject.transform); }
    }
}
