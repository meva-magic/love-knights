using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public Slider volumeSlider;
    
    private const string MOUSE_SENS_KEY = "MouseSensitivity";
    private const string VOLUME_KEY = "GameVolume";

    void Start()
    {
        // Load saved settings or use defaults
        float savedSens = PlayerPrefs.GetFloat(MOUSE_SENS_KEY, 2f);
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.8f);
        
        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.value = savedSens;
            mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        }
        
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
            AudioListener.volume = savedVolume;
        }
    }

    public void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat(MOUSE_SENS_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }
}
