using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TAG_Enemy>() != null)
        {
            //Debug.Log("is enemy: " + other.name);

            other.GetComponent<HealthSystem>().DealDamage(damage);

            if (audioClips.Length > 0)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.Play();
            }
        }
    }
}
