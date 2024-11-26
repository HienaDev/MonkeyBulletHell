using System.Collections;
using UnityEngine;

public class ArenaActivateDelay : MonoBehaviour
{

    [SerializeField] private GameObject arena;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
