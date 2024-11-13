using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq.Expressions;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class ShootingPlayer : MonoBehaviour
{

    [SerializeField] private KeyCode shoot = KeyCode.Mouse0;

    private float shotSpeed = 200f;
    [SerializeField] private float fireRate = 0.1f;
    private float justShot;
    private Animator animator;

    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject shotManager;
    private bool laserLikeProjectile = false;
    private float laserDuration = 0f;
    private float laserStarted = 0f;
    private bool laserFiring = false;
    private GameObject shotPrefab;
    private List<GameObject> instantiatedShots;
    private int currentShot = 0;

    [SerializeField] private WeaponSO testWeapon;
    private WeaponSO currentWeapon;


    private AudioSource audioSource;
    private AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {

        justShot = Time.time;

        animator = GetComponent<Animator>();

        instantiatedShots = new List<GameObject>();

        audioSource = GetComponent<AudioSource>();

        if(testWeapon != null)
            SetWeapon(testWeapon);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(shoot))
        {
            if (!laserLikeProjectile && Time.time - justShot > fireRate && instantiatedShots.Count > 0)
                ShootProjectiles();
        }

        if (Input.GetKeyDown(shoot) && laserLikeProjectile && Time.time - justShot > fireRate && !laserFiring)
        {
            laserStarted = Time.time;
            laserFiring = true;
            Debug.Log("startlaser");
            StartLaser();
        }
        if(Input.GetKey(shoot) && laserLikeProjectile)
        {
            AimLaser();
        }
        if (((Input.GetKeyUp(shoot) && laserLikeProjectile) || Time.time - laserStarted > laserDuration) && laserFiring)
        {
            Debug.Log("stoplaser");
            justShot = Time.time;
            laserFiring = false;
            StopLaser();
        }
    }

    private void ShootProjectiles()
    {

        instantiatedShots[currentShot].transform.position = firePoint.transform.position;

        Vector3 direction = Mouse3D.GetMouseObjectPosition() - instantiatedShots[currentShot].transform.position;
        instantiatedShots[currentShot].transform.up = direction;

        instantiatedShots[currentShot].SetActive(true);

        instantiatedShots[currentShot].GetComponent<Rigidbody>().linearVelocity = instantiatedShots[currentShot].transform.up * shotSpeed;

        currentShot++;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        if (currentShot >= instantiatedShots.Count)
        {
            currentShot = 0;
        }

        animator.SetTrigger("Shoot");
        justShot = Time.time;
    }

    private void StartLaser()
    {
        instantiatedShots[0].SetActive(true);
    }

    private void AimLaser()
    {
        Vector3 direction = Mouse3D.GetMouseObjectPosition() - instantiatedShots[currentShot].transform.position;

        instantiatedShots[currentShot].transform.position = firePoint.transform.position;

        float yAngle = GetYAngleToTarget(direction);

        instantiatedShots[currentShot].transform.eulerAngles = new Vector3 (0, yAngle, 0);
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

    private void StopLaser()
    {
        instantiatedShots[0].SetActive(false);
    }

    public void ResetTrigger()
    {
        animator.ResetTrigger("Shoot");

    }

    private IEnumerator CreateShots()
    {
        if(laserLikeProjectile)
        {
            GameObject shotClone;

            shotClone = Instantiate(shotPrefab, shotManager.transform);


            shotClone.SetActive(false);
            instantiatedShots.Add(shotClone);
            yield return null;
        }
        else
        {
            for (int i = 0; i < 10 / fireRate; i++)
            {
                GameObject shotClone;

                shotClone = Instantiate(shotPrefab, shotManager.transform);


                shotClone.SetActive(false);
                instantiatedShots.Add(shotClone);
                yield return null;
            }
        }
        

    }

    public void SetWeapon(WeaponSO weapon)
    {
        foreach(GameObject shot in instantiatedShots)
        {
            Destroy(shot);
        }
        Debug.Log("weapon equipped");

        instantiatedShots = new List<GameObject>();
        currentWeapon = weapon;
        fireRate = weapon.fireRate;
        shotSpeed = weapon.shotSpeed;
        shotPrefab = weapon.projectilePrefab;
        laserLikeProjectile = weapon.laserLikeProjectile;
        laserDuration = weapon.laserDuration;
        audioClips = weapon.shootingSounds;
        currentShot = 0;

        StartCoroutine(CreateShots());
    }
}
