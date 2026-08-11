using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int current = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutions[i].width + " x " + resolutions[i].height));

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                current = i;
            }
        }

        resolutionDropdown.value = current;
        resolutionDropdown.RefreshShownValue();

        musicSlider.value = PlayerPrefs.GetFloat("Music", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1f);
    }

    public void SetResolution(int index)
    {
        Resolution r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);

        PlayerPrefs.SetInt("Resolution", index);
    }

    public void SetMusic(float value)
    {
        PlayerPrefs.SetFloat("Music", value);
    }

    public void SetSFX(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
    }
}