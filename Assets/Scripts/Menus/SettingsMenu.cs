using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    //[SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown   resolutionDropdown;
    [SerializeField] private Slider         sensitivitySlider;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioMixer     audioMixer;

    private Resolution[] resolutions;

    private void Start()
    {   
        resolutions = Screen.resolutions
            .Where(resolution => Mathf.Approximately((float)resolution.width / resolution.height, 16f / 9f))
            .Select(resolution => new Resolution { width = resolution.width, height = resolution.height })
            .Distinct()
            .ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //sensitivitySlider.value = playerMovement.GetSensitivity();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSensitive(float sensitivity)
    {
        //playerMovement.SetSensitivity(sensitivity);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void BackButton()
    {
        this.gameObject.SetActive(false);
    }
}
