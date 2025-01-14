using UnityEngine;
using UnityEngine.Audio;

public class AnimationSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepsSound;
    private AudioSource audioSourceSteps;
    [SerializeField] private AudioMixerGroup stepsMixer;

    [SerializeField] private AudioClip[] Snd1Sound;
    private AudioSource audioSourceSnd1;
    [SerializeField] private AudioMixerGroup Snd1Mixer;

    [SerializeField] private AudioClip[] Snd2Sound;
    private AudioSource audioSourceSnd2;
    [SerializeField] private AudioMixerGroup Snd2Mixer;

    [SerializeField] private AudioClip[] Snd3Sound;
    private AudioSource audioSourceSnd3;
    [SerializeField] private AudioMixerGroup Snd3Mixer;

    [SerializeField] private AudioClip[] Snd4Sound;
    private AudioSource audioSourceSnd4;
    [SerializeField] private AudioMixerGroup Snd4Mixer;

    [SerializeField] private AudioClip[] Snd5Sound;
    public AudioSource BossMusic => audioSourceSnd5;
    private AudioSource audioSourceSnd5;
    [SerializeField] private AudioMixerGroup Snd5Mixer;

    [SerializeField] private AudioClip[] Snd6Sound;

    private AudioSource audioSourceSnd6;
    [SerializeField] private AudioMixerGroup Snd6Mixer;

    [SerializeField] private AudioClip[] Snd7Sound;

    private AudioSource audioSourceSnd7;
    [SerializeField] private AudioMixerGroup Snd7Mixer;

    [SerializeField] private AudioClip[] Snd8Sound;

    private AudioSource audioSourceSnd8;
    [SerializeField] private AudioMixerGroup Snd8Mixer;

    [SerializeField] private AudioClip[] Snd9Sound;

    private AudioSource audioSourceSnd9;
    [SerializeField] private AudioMixerGroup Snd9Mixer;

    [SerializeField] private AudioClip[] Snd10Sound;

    private AudioSource audioSourceSnd10;
    [SerializeField] private AudioMixerGroup Snd10Mixer;

    [SerializeField] private AudioClip[] Snd11Sound;

    private AudioSource audioSourceSnd11;
    [SerializeField] private AudioMixerGroup Snd11Mixer;


    void Start()
    {
        audioSourceSteps = gameObject.AddComponent<AudioSource>();
        audioSourceSteps.outputAudioMixerGroup = stepsMixer;
        //AudioManager.instance.audioSources.Add(audioSourceSteps);

        audioSourceSnd1 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd1.outputAudioMixerGroup = Snd1Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd1);

        audioSourceSnd2 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd2.outputAudioMixerGroup = Snd2Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd2);

        audioSourceSnd3 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd3.outputAudioMixerGroup = Snd3Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd3);

        audioSourceSnd4 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd4.outputAudioMixerGroup = Snd4Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd4);

        audioSourceSnd5 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd5.outputAudioMixerGroup = Snd5Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd5);
        audioSourceSnd6 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd6.outputAudioMixerGroup = Snd6Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd6);

        audioSourceSnd7 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd7.outputAudioMixerGroup = Snd7Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd6);

        audioSourceSnd8 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd8.outputAudioMixerGroup = Snd8Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd6);

        audioSourceSnd9 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd9.outputAudioMixerGroup = Snd9Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd6);

        audioSourceSnd10 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd10.outputAudioMixerGroup = Snd10Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd6);

        audioSourceSnd11 = gameObject.AddComponent<AudioSource>();
        audioSourceSnd11.outputAudioMixerGroup = Snd11Mixer;
        //AudioManager.instance.audioSources.Add(audioSourceSnd6);

        audioSourceSnd1.spatialBlend = 0f;
        audioSourceSteps.spatialBlend = 0f;
        audioSourceSnd2.spatialBlend = 0f;
        audioSourceSnd3.spatialBlend = 0f;
        audioSourceSnd4.spatialBlend = 0f;
        audioSourceSnd5.spatialBlend = 0f;
        audioSourceSnd6.spatialBlend = 0f;
        audioSourceSnd7.spatialBlend = 0f;
        audioSourceSnd8.spatialBlend = 0f;
        audioSourceSnd9.spatialBlend = 0f;
        audioSourceSnd10.spatialBlend = 0f;
        audioSourceSnd11.spatialBlend = 0f;

        audioSourceSnd5.loop = true;

        audioSourceSnd4.minDistance = 0.1f;
        audioSourceSnd4.maxDistance = 10f;

        audioSourceSnd1.volume = 0.07f;
        audioSourceSteps.volume = 1f;
        audioSourceSnd2.volume = 1f;
        audioSourceSnd3.volume = 1f;
        audioSourceSnd4.volume = 1f;
        audioSourceSnd5.volume = 1f;
        audioSourceSnd6.volume = 1f;
        audioSourceSnd7.volume = 1f;
        audioSourceSnd8.volume = 1f;
        audioSourceSnd9.volume = 1f;
        audioSourceSnd10.volume = 1f;
        audioSourceSnd11.volume = 1f;


        
    }

    public void PlaySnd1Sound()
    {
        audioSourceSnd1.clip = Snd1Sound[Random.Range(0, Snd1Sound.Length)];
        audioSourceSnd1.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd1.Play();
    }

    public void PlaySnd2Sound()
    {
        audioSourceSnd2.clip = Snd2Sound[Random.Range(0, Snd2Sound.Length)];
        audioSourceSnd2.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd2.Play();
    }

    public void PlaySnd3Sound()
    {
        audioSourceSnd3.clip = Snd3Sound[Random.Range(0, Snd3Sound.Length)];
        audioSourceSnd3.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd3.Play();
    }

    public void PlaySnd4Sound()
    {
        audioSourceSnd4.clip = Snd4Sound[Random.Range(0, Snd4Sound.Length)];
        audioSourceSnd4.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd4.Play();
    }

    public void PlaySnd5Sound()
    {
        audioSourceSnd5.clip = Snd5Sound[Random.Range(0, Snd5Sound.Length)];
        audioSourceSnd5.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd5.Play();
    }

    public void ResetSnd5()
    {
        audioSourceSnd5.Stop();
    }

    public void PlaySnd6Sound()
    {
        audioSourceSnd6.clip = Snd6Sound[Random.Range(0, Snd6Sound.Length)];
        audioSourceSnd6.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd6.Play();
    }

     public void PlaySnd7Sound()
    {
        audioSourceSnd7.clip = Snd7Sound[Random.Range(0, Snd7Sound.Length)];
        audioSourceSnd7.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd7.Play();
    }

    public void PlaySnd8Sound()
    {
        audioSourceSnd8.clip = Snd8Sound[Random.Range(0, Snd8Sound.Length)];
        audioSourceSnd8.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd8.Play();
        
    }
    public void PlaySnd9Sound()
    {
        audioSourceSnd9.clip = Snd9Sound[Random.Range(0, Snd9Sound.Length)];
        audioSourceSnd9.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd9.Play();
        
    }
    public void PlaySnd10Sound()
    {
        audioSourceSnd10.clip = Snd10Sound[Random.Range(0, Snd10Sound.Length)];
        audioSourceSnd10.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd10.Play();
        
    }

    public void PlaySnd11Sound()
    {
        audioSourceSnd11.clip = Snd11Sound[Random.Range(0, Snd11Sound.Length)];
        audioSourceSnd11.pitch = Random.Range(0.95f, 1.05f);

        audioSourceSnd11.Play();
        
    }
    
}
