using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ControlsMenu : MonoBehaviour
{
	public TMP_Text[] buttonTexts;
	
	public TMP_Text[] labelTexts;
	
	public GameObject panel;
	
	public float showTime;
	
	public int currentClick;
	
	public TMP_Text counterText;
	
	void OnEnable()
	{
		this.showTime = 0f;
		this.UpdateAllTexts();
	}
	
	public void UpdateAllTexts()
	{
		for (int i = 0; i < buttonTexts.Length; i++)
		{
			KeyCode key = Singleton<InputManager>.Instance.KeyboardMapping[(InputAction)i];
			buttonTexts[i].text = key.ToString();
		}
	}
	
	public void ShowScreen(int num)
	{
		this.panel.SetActive(true);
		this.showTime = 5f;
		this.currentClick = num;
		foreach (Button but in UnityEngine.Object.FindObjectsOfType<Button>()) but.interactable = false;
	}
	
	public void SetDefaults()
	{
		Singleton<InputManager>.Instance.SetDefaults();
		Singleton<InputManager>.Instance.Save("DoNotShip");
		this.UpdateAllTexts();
	}
	
	void Update()
	{
		if (this.showTime > 0f)
		{
			this.counterText.text = string.Format("Press a key to assign it to {0} in {1} seconds.", labelTexts[currentClick].text, Mathf.CeilToInt(showTime).ToString());
			if (Input.GetMouseButtonDown(0))
			{
				this.showTime = 0f;
				if (Input.GetMouseButton(1))
				{
					Singleton<InputManager>.Instance.Modifiy((InputAction)currentClick, KeyCode.None);
					Singleton<InputManager>.Instance.Save("DoNotShip");
					this.UpdateAllTexts();
				}
			}
			foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKeyDown(kcode) & !kcode.ToString().Contains("Mouse"))
				{
					this.showTime = 0f;
					Singleton<InputManager>.Instance.Modifiy((InputAction)currentClick, kcode);
					Singleton<InputManager>.Instance.Save("DoNotShip");
					Debug.Log(labelTexts[currentClick].text + " setting has been changed to " + kcode.ToString());
					this.UpdateAllTexts();
				}
			}
			this.showTime -= Time.unscaledDeltaTime;
		}
		else if (this.panel.activeSelf)
		{
			this.panel.SetActive(false);
			foreach (Button but in UnityEngine.Object.FindObjectsOfType<Button>()) but.interactable = true;
		}
	}
}