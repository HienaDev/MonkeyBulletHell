using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;

public class MoaiBossFight : MonoBehaviour
{
    [SerializeField] private DecalProjector target;
    [SerializeField, ColorUsage(true, true)] private Color goneColor;
    [SerializeField, ColorUsage(true, true)] private Color initialColor;
    [SerializeField, ColorUsage(true, true)] private Color halfWayColor;
    [SerializeField, ColorUsage(true, true)] private Color finalColor;
    [SerializeField] private GameObject groundCracksParent;
    [SerializeField] private DecalProjector hole1;
    [SerializeField] private DecalProjector hole2;
    [SerializeField] private DecalProjector cracks;
    [SerializeField] private DecalProjector crackSingle1;
    [SerializeField] private GameObject crackSingle1Collider;
    [SerializeField] private DecalProjector crackSingle2;
    [SerializeField] private GameObject crackSingle2Collider;
    [SerializeField] private DecalProjector crackSingle3;
    [SerializeField] private GameObject crackSingle3Collider;
    [SerializeField] private DecalProjector crackSingle4;
    [SerializeField] private GameObject crackSingle4Collider;
    [SerializeField] private DecalProjector crackSingle5;
    [SerializeField] private GameObject crackSingle5Collider;
    [SerializeField] private DecalProjector crackSingle6;
    [SerializeField] private GameObject crackSingle6Collider;
    private List<DecalProjector> cracksObjects;
    private List<GameObject> collidersCracks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cracksObjects = new List<DecalProjector>
        {
            hole1,
            hole2,
            cracks,
            crackSingle1,
            crackSingle2,
            crackSingle3,
            crackSingle4,
            crackSingle5,
            crackSingle6
        };

        collidersCracks = new List<GameObject>
        {
            crackSingle1Collider,
            crackSingle2Collider,
            crackSingle3Collider,
            crackSingle4Collider,
            crackSingle5Collider,
            crackSingle6Collider
        };

        foreach (DecalProjector crack in cracksObjects)
        {
            crack.material.SetColor("_Color", goneColor);
        }

        //TriggerCracksOnGround();

    }

    public void TriggerCracksOnGround()
    {
        StartCoroutine(CracksOnGroundCR());
    }

    private IEnumerator CracksOnGroundCR()
    {
        Debug.Log("Trigger cracks");
        groundCracksParent.SetActive(true);

        yield return new WaitForSeconds(2f);

        foreach (DecalProjector crack in cracksObjects)
        {

            crack.material.SetColor("_Color", initialColor);
        }

        float lerpValue = 0f;

        while(lerpValue < 1f)
        {
            lerpValue += Time.deltaTime / 2;
            foreach (DecalProjector crack in cracksObjects)
            {

                crack.material.SetColor("_Color", Color.Lerp(initialColor, halfWayColor, lerpValue));
            }
            yield return null;
        }

        foreach (DecalProjector crack in cracksObjects)
        {
            crack.material.SetColor("_Color", finalColor);
        }

        foreach (GameObject collider in collidersCracks)
        {
            collider.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (DecalProjector crack in cracksObjects)
        {
            crack.material.SetColor("_Color", goneColor);
        }

        foreach (GameObject collider in collidersCracks)
        {
            collider.SetActive(false);
        }
    }
}
