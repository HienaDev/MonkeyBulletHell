using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool Grounded {  get; private set; }

    private void Start()
    {
        Grounded = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Grounded = false;
    }
}
