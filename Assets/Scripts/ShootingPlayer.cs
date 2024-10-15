using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ShootingPlayer : MonoBehaviour
{

    [SerializeField] private KeyCode shoot = KeyCode.Mouse0;

    [SerializeField] private float shotSpeed = 200f;
    [SerializeField] private float fireRate = 0.5f;
    private float justShot;
    private Animator animator;

    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject shotManager;
    [SerializeField] private GameObject shotPrefab;
    private List<GameObject> instantiatedShots;
    private int currentShot = 0;

    // Start is called before the first frame update
    void Start()
    {

        justShot = Time.time;

        animator = GetComponent<Animator>();

        instantiatedShots = new List<GameObject>();

        StartCoroutine(CreateShots());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(shoot) && Time.time - justShot > fireRate)
        {
            Shoot();

        }
    }

    private void Shoot()
    {
        instantiatedShots[currentShot].transform.position = firePoint.transform.position;

        Vector3 direction = Mouse3D.GetMouseObjectPosition() - instantiatedShots[currentShot].transform.position;
        instantiatedShots[currentShot].transform.up = direction;

        instantiatedShots[currentShot].SetActive(true);

        instantiatedShots[currentShot].GetComponent<Rigidbody>().linearVelocity = instantiatedShots[currentShot].transform.up * shotSpeed;

        currentShot++;

        if (currentShot >= instantiatedShots.Count)
        {
            currentShot = 0;
        }

        animator.SetTrigger("Shoot");
        justShot = Time.time;
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
}
