using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
