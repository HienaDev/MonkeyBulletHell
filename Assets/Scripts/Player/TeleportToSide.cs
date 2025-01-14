using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DashInDirection : MonoBehaviour
{
    [SerializeField] private float dashCooldown = 2f;
    private float justDashed;
    [SerializeField] private float dashDistance = 5f; // Distance to dash (5 meters)
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash (in seconds)
    [SerializeField] private float gracePeriod = 0.1f; // Duration of the dash (in seconds)
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // Key to trigger the dash (Left Shift)
    [SerializeField] private KeyCode alternateDashKey = KeyCode.LeftShift; // Key to trigger the dash (Left Shift)
    [SerializeField] private GameObject characterModel; // Reference to the model for rotation

    [SerializeField] private Image dashUI;

    private bool isDashing = false; // To prevent multiple dashes at once

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Collider hitBox;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    private PlayerMovement playerMovement;


    [SerializeField] private Material[] dashMaterials;
    [SerializeField] private Material[] normalMaterials;
    
    public AudioClip[] DashSounds;

    private AudioSource audioSource;
    private AudioClip[] audioClips;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        justDashed = Time.time;
        trailRenderer.emitting = false;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        audioClips = DashSounds;
    }

    private void Update()
    {
        // Check if the dash key is pressed and we're not already dashing
        if ((Input.GetKeyDown(dashKey) || Input.GetKeyDown(alternateDashKey)) && !isDashing && Time.time - justDashed > dashCooldown && playerMovement.Velocity.magnitude > 0.2f)
        {
            justDashed = Time.time;
            StartCoroutine(Dash());
            audioSource.clip = audioClips[0];
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        float dashTimer = Time.time - justDashed;
        if (dashTimer <= dashCooldown + 0.1f) // +0.1f so it fills up completely
        {
            dashUI.fillAmount = dashTimer / dashCooldown;   
        }
    }

    private void TurnTransparent()
    {
        skinnedMeshRenderer.materials = dashMaterials;
    }

    private void TurnSolid()
    {
        skinnedMeshRenderer.materials = normalMaterials;
    }

    private IEnumerator Dash()
    {
        trailRenderer.emitting = true;
        hitBox.enabled = false;

        TurnTransparent();

        isDashing = true;

        //// Get the direction based on the characterModel's forward direction
        //Vector3 dashDirection = -characterModel.transform.up;

        //// Calculate the target position
        //Vector3 startPosition = transform.position;
        //Vector3 targetPosition = startPosition + dashDirection * dashDistance;

        //// Initialize time tracking
        //float elapsedTime = 0f;

        //// Move smoothly over the dashDuration
        //while (elapsedTime < dashDuration)
        //{
        //    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);
        //    elapsedTime += Time.deltaTime;

        //    yield return null;
        //}

        playerMovement.MultiplySpeed(dashDistance);

        yield return new WaitForSeconds(dashDuration);
        playerMovement.ResetSpeed();

        yield return new WaitForSeconds(gracePeriod);

        isDashing = false;

        trailRenderer.emitting = false;
        hitBox.enabled = true;

        TurnSolid();
    }
}
