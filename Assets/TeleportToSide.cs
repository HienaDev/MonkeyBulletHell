using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using NUnit.Framework;
using System.Linq;

public class DashInDirection : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f; // Distance to dash (5 meters)
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash (in seconds)
    [SerializeField] private float gracePeriod = 0.1f; // Duration of the dash (in seconds)
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // Key to trigger the dash (Left Shift)
    [SerializeField] private GameObject characterModel; // Reference to the model for rotation

    private bool isDashing = false; // To prevent multiple dashes at once

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Collider hitBox;

    private SkinnedMeshRenderer skinnedMeshRenderer;


    [SerializeField] private Material[] dashMaterials;
    [SerializeField] private Material[] normalMaterials;

    private void Start()
    {
        trailRenderer.emitting = false;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

    }

    private void Update()
    {
        // Check if the dash key is pressed and we're not already dashing
        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            StartCoroutine(Dash());
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

        // Get the direction based on the characterModel's forward direction
        Vector3 dashDirection = -characterModel.transform.up;

        // Calculate the target position
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + dashDirection * dashDistance;

        // Initialize time tracking
        float elapsedTime = 0f;

        // Move smoothly over the dashDuration
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }


        yield return new WaitForSeconds(gracePeriod);
        // Ensure the object ends exactly at the target position

        isDashing = false;

        trailRenderer.emitting = false;
        hitBox.enabled = true;

        TurnSolid();
    }
}
