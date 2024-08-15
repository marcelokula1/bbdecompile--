using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

// Token: 0x0200001B RID: 27
public class OptionsManager : MonoBehaviour
{
	// Token: 0x06000061 RID: 97 RVA: 0x000037B4 File Offset: 0x00001BB4
	private void Start()
	{
		if (PlayerPrefs.HasKey("OptionsSet"))
		{
			slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
			if (PlayerPrefs.GetInt("Rumble") == 1)
			{
				rumble.isOn = true;
			}
			else
			{
				rumble.isOn = false;
			}
			if (PlayerPrefs.GetInt("AnalogMove") == 1)
			{
				analog.isOn = true;
			}
			else
			{
				analog.isOn = false;
			}
			if (PlayerPrefs.GetInt("V-Sync") == 0)
            {
                this.Vsync.isOn = false;
				QualitySettings.vSyncCount = 0;
            }
            else
            {
                this.Vsync.isOn = true;
				QualitySettings.vSyncCount = 1;
            }
		}
		else
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
		}

		if (!PlayerPrefs.HasKey("musicVolume"))
		{
			PlayerPrefs.SetFloat("musicVolume", 1f);
			Load();
		}
		else
		{
			Load();
		}

		this.resolutions = Screen.resolutions;
		this.resolutionDropdown.ClearOptions();
		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < this.resolutions.Length; i++)
		{
			string item = this.resolutions[i].width.ToString() + " × " + this.resolutions[i].height.ToString();
			list.Add(item);
			if (this.resolutions[i].width == Screen.currentResolution.width && this.resolutions[i].height == Screen.currentResolution.height)
			{
				value = i;
			}
		}
		this.resolutionDropdown.AddOptions(list);
		this.resolutionDropdown.value = value;
		this.resolutionDropdown.RefreshShownValue();

		if (!PlayerPrefs.HasKey("OptionsSet"))
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
			return;
		}

		if (PlayerPrefs.GetInt("isFullScreen") == 1)
		{
			this.isFullScreen.isOn = true;
			return;
		}
		this.isFullScreen.isOn = false;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003850 File Offset: 0x00001C50
	private void Update()
	{
		PlayerPrefs.SetFloat("MouseSensitivity", slider.value);
		if (rumble.isOn)
		{
			PlayerPrefs.SetInt("Rumble", 1);
		}
		else
		{
			PlayerPrefs.SetInt("Rumble", 0);
		}
		if (analog.isOn)
		{
			PlayerPrefs.SetInt("AnalogMove", 1);
		}
		else
		{
			PlayerPrefs.SetInt("AnalogMove", 0);
		}
		if (this.Vsync.isOn)
        {
            PlayerPrefs.SetInt("V-Sync", 1);
			QualitySettings.vSyncCount = 1;
        }
        else
        {
            PlayerPrefs.SetInt("V-Sync", 0);
			QualitySettings.vSyncCount = 0;
        }
		if (this.isFullScreen.isOn)
		{
			PlayerPrefs.SetInt("isFullScreen", 1);
			return;
		}
		PlayerPrefs.SetInt("isFullScreen", 0);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002887 File Offset: 0x00000A87
	public void ChangeVolume()
	{
		AudioListener.volume = volumeSlider.value;
		Save();
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x000268A8 File Offset: 0x00024CA8
	private void Load()
	{
		volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
	}

    // Token: 0x060009F2 RID: 2546 RVA: 0x00026992 File Offset: 0x00024D92
	private void Save()
	{
		PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000028AD File Offset: 0x00000AAD
	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000028B8 File Offset: 0x00000AB8
	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = this.resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	// Token: 0x0400006F RID: 111
	public Slider slider;

	// Token: 0x04000070 RID: 112
	public Toggle rumble;

	// Token: 0x04000071 RID: 113
	public Toggle analog;

	// Token: 0x04000072 RID: 114
	public Toggle Vsync;

	// Token: 0x04000073 RID: 115
	[SerializeField]
	private Slider volumeSlider;

	// Token: 0x04000074 RID: 116
	private Resolution[] resolutions;

	// Token: 0x04000075 RID: 117
	public TMP_Dropdown resolutionDropdown;

	// Token: 0x04000076 RID: 118
	public Toggle isFullScreen;
}
