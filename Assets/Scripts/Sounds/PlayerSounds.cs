using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepsSound;
    private AudioSource audioSourceSteps;
    [SerializeField] private AudioMixerGroup stepsMixer;

    [SerializeField] private AudioClip[] takePictureSound;
    private AudioSource audioSourceTakePicture;
    [SerializeField] private AudioMixerGroup takePictureMixer;

    [SerializeField] private AudioClip[] stepsGravelSound;
    private AudioSource audioSourceStepsGravel;
    [SerializeField] private AudioMixerGroup stepsGravelMixer;

    [SerializeField] private AudioClip[] blockBreakingSound;
    private AudioSource audioSourceBlockBreaking;
    [SerializeField] private AudioMixerGroup blockBreakingMixer;

    [SerializeField] private AudioClip[] pickUpSound;
    private AudioSource audioSourcePickUp;
    [SerializeField] private AudioMixerGroup pickUpMixer;

    [SerializeField] private AudioClip[] menuSound;
    private AudioSource audioSourceMenu;
    [SerializeField] private AudioMixerGroup menuMixer;

    [SerializeField] private float timeBetweenSteps;

    private PlayerMovement playerScript;

    private float justStep;
    public bool OnGravel = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSourceSteps = gameObject.AddComponent<AudioSource>();
        audioSourceSteps.outputAudioMixerGroup = stepsMixer;
        AudioManager.instance.audioSources.Add(audioSourceSteps);

        audioSourceTakePicture = gameObject.AddComponent<AudioSource>();
        audioSourceTakePicture.outputAudioMixerGroup = takePictureMixer;
        AudioManager.instance.audioSources.Add(audioSourceTakePicture);

        audioSourceStepsGravel = gameObject.AddComponent<AudioSource>();
        audioSourceStepsGravel.outputAudioMixerGroup = stepsGravelMixer;
        AudioManager.instance.audioSources.Add(audioSourceStepsGravel);

        audioSourceBlockBreaking = gameObject.AddComponent<AudioSource>();
        audioSourceBlockBreaking.outputAudioMixerGroup = blockBreakingMixer;
        AudioManager.instance.audioSources.Add(audioSourceBlockBreaking);

        audioSourcePickUp = gameObject.AddComponent<AudioSource>();
        audioSourcePickUp.outputAudioMixerGroup = pickUpMixer;
        AudioManager.instance.audioSources.Add(audioSourcePickUp);

        audioSourceMenu = gameObject.AddComponent<AudioSource>();
        audioSourceMenu.outputAudioMixerGroup = menuMixer;
        AudioManager.instance.audioSources.Add(audioSourceMenu);

        audioSourceTakePicture.spatialBlend = 1;
        audioSourceSteps.spatialBlend = 1;
        audioSourceStepsGravel.spatialBlend = 1;
        audioSourceBlockBreaking.spatialBlend = 1;
        audioSourcePickUp.spatialBlend = 0.98f;
        audioSourceMenu.spatialBlend = 1;

        audioSourcePickUp.minDistance = 0.1f;
        audioSourcePickUp.maxDistance = 10f;

        audioSourceTakePicture.volume = 0.2f;
        audioSourceSteps.volume = 0.1f;
        audioSourceStepsGravel.volume = 0.1f;
        audioSourceBlockBreaking.volume = 3f;
        audioSourcePickUp.volume = 1f;
        audioSourceMenu.volume = 0.1f;


        //audioSourcePickUp.clip = pickUpSound;


        playerScript = GetComponent<PlayerMovement>();

        justStep = timeBetweenSteps;



    }


   
    

    

    public void PlayStepsSound()
    {
        audioSourceSteps.clip = stepsSound[Random.Range(0, stepsSound.Length)];
        audioSourceSteps.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSteps.Play();
    }

    public void PlayTakePictureSound()
    {
        audioSourceTakePicture.clip = takePictureSound[Random.Range(0, takePictureSound.Length)];
        audioSourceTakePicture.pitch = Random.Range(0.95f, 1.05f);

        audioSourceTakePicture.Play();
    }

    public void PlayStepsGravelSound()
    {
        audioSourceStepsGravel.clip = stepsGravelSound[Random.Range(0, stepsGravelSound.Length)];
        audioSourceStepsGravel.pitch = Random.Range(0.95f, 1.05f);

        audioSourceStepsGravel.Play();
    }

    public void PlayBlockBreakingSound()
    {
        audioSourceBlockBreaking.clip = blockBreakingSound[Random.Range(0, blockBreakingSound.Length)];
        audioSourceBlockBreaking.pitch = Random.Range(0.95f, 1.05f);

        audioSourceBlockBreaking.Play();
    }

    public void PlayPickUpSound()
    {
        audioSourcePickUp.clip = pickUpSound[Random.Range(0, pickUpSound.Length)];
        audioSourcePickUp.pitch = Random.Range(0.95f, 1.05f);

        audioSourcePickUp.Play();
    }

    public void PlayMenuSound()
    {
        audioSourceMenu.clip = menuSound[Random.Range(0, menuSound.Length)];
        audioSourceMenu.pitch = Random.Range(0.95f, 1.05f);

        audioSourceMenu.Play();
    }

}
