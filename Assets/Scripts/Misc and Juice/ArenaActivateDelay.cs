using System.Collections;
using UnityEngine;

public class ArenaActivateDelay : MonoBehaviour
{
    [SerializeField] private GameObject arena;

    public void ArenaDelayCR()
    {
        StartCoroutine(ArenaDelay());
    }

    private IEnumerator ArenaDelay()
    {
        yield return new WaitForSeconds(10f);

        arena.SetActive(true);
    }
}
