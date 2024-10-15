using UnityEngine;

public class TAG_ShotManager_SINGLETON : MonoBehaviour
{
    // Static instance of the Singleton
    public static TAG_ShotManager_SINGLETON Instance { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Set the instance to this instance
        Instance = this;
    }
}
