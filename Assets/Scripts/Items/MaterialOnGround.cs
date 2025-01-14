using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class MaterialOnGround : MonoBehaviour
{
    [SerializeField] private MaterialSO material;

    private PlayerInventory playerInventory;

    private AudioSource audioSource;
    private AudioClip[] audioClips;

    private void Start()
    {
        playerInventory = PlayerInventory.Instance;

        audioSource = GetComponent<AudioSource>();
    }

    public void GatherMaterial()
    {
        audioClips = material.pickupSounds;
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        if (playerInventory.ContainsMaterial(material))
        {
            playerInventory.AddItem(material);
            Debug.Log($"Added {material.itemName} to existing stack in inventory.");
        }
        else if (playerInventory.MaterialAndToolSlotsFull())
        {
            Debug.Log("Inventory full. Cannot pick up material.");
            return;
        }
        else
        {
            playerInventory.AddItem(material);
            Debug.Log($"Gathered {material.itemName}");
            
        }

        transform.position = new Vector3(1.0f, 1.0f, 1.0f);
        Invoke("Destroy_object",3);
        
        
    }

    private void Destroy_object()
    {
        Destroy(gameObject);
    }

    public void SetMaterial(MaterialSO newMaterial)
    {
        material = newMaterial;
    }
}