using UnityEngine;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public enum InputAction
{
    MoveForward = 0,
    MoveBackward = 1,
    MoveLeft = 2,
    MoveRight = 3,
    Run = 4,
	LookBehindJump = 5,
	PauseOrCancel = 6,
	Interact = 7,
	UseItem = 8,
	Slot0 = 9,
	Slot1 = 10,
	Slot2 = 11,
}

public class InputManager : Singleton<InputManager>
{

    public Dictionary<InputAction, KeyCode> KeyboardMapping = new Dictionary<InputAction, KeyCode>()
    {
        { InputAction.MoveForward, KeyCode.W },
        { InputAction.MoveBackward, KeyCode.S },
        { InputAction.MoveLeft, KeyCode.A },
        { InputAction.MoveRight, KeyCode.D },
        { InputAction.Run, KeyCode.LeftShift },
        { InputAction.LookBehindJump, KeyCode.Space },
		{ InputAction.PauseOrCancel, KeyCode.Escape },
		{ InputAction.Interact, KeyCode.E },
		{ InputAction.UseItem, KeyCode.Q },
		{ InputAction.Slot0, KeyCode.Alpha1 },
		{ InputAction.Slot1, KeyCode.Alpha2 },
		{ InputAction.Slot2, KeyCode.Alpha3 }
    };

	
	public void Modifiy(InputAction type, KeyCode newer)
	{
		KeyboardMapping[type] = newer;
	}
	
	private Dictionary<InputAction, bool> keyStates = new Dictionary<InputAction, bool>();

    public bool GetActionKey(InputAction action)
    {
        if (KeyboardMapping.ContainsKey(action))
        {
			bool isKeyPressed = Input.GetKey(KeyboardMapping[action]);

            if (action == InputAction.Run)
            {
                return isKeyPressed;
            }

			if (action == InputAction.LookBehindJump)
            {
                return isKeyPressed;
            }

			if (action == InputAction.MoveForward)
            {
                return isKeyPressed;
            }

			if (action == InputAction.MoveBackward)
            {
                return isKeyPressed;
            }

			if (action == InputAction.MoveRight)
            {
                return isKeyPressed;
            }

			if (action == InputAction.MoveLeft)
            {
                return isKeyPressed;
            }

			if (action == InputAction.MoveLeft)
            {
                return isKeyPressed;
            }

			if (action == InputAction.Interact)
            {
                return isKeyPressed;
            }

            if (isKeyPressed && !keyStates.ContainsKey(action))
            {
                keyStates[action] = true;
                return true;
            }

            if (!isKeyPressed && keyStates.ContainsKey(action))
            {
                keyStates.Remove(action);
            }

            return false;
        }

        return false;
    }

    public bool GetActionKeyDown(InputAction action)
    {
        if (KeyboardMapping.ContainsKey(action) && Input.GetKeyDown(KeyboardMapping[action]))
        {
            keyStates[action] = true;
            return true;
        }

        return false;
    }

    public bool GetActionKeyUp(InputAction action)
    {
        if (KeyboardMapping.ContainsKey(action) && Input.GetKeyUp(KeyboardMapping[action]))
        {
            keyStates[action] = false;
            return true;
        }

        return false;
    }
 
	
	public void Save(string fileName)
	{
		string filePath = Application.persistentDataPath + "/Controls_" + fileName + ".xml";
		XmlSerializer serializer = new XmlSerializer(typeof(List<InputMapping>));

		List<InputMapping> inputMappings = new List<InputMapping>();
		foreach (var kvp in KeyboardMapping)
		{
            InputMapping mapping = new InputMapping
            {
                Action = kvp.Key,
                KeyCode = kvp.Value
            };
            inputMappings.Add(mapping);
		}

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, inputMappings);
        }
    }

	public void Load(string fileName)
	{
		string filePath = Application.persistentDataPath + "/Controls_" + fileName + ".xml";
		string oldPath = Application.persistentDataPath + "/Controls_" + fileName + ".dat";
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		if (File.Exists(oldPath))
		{
			FileStream fileStream2 = File.OpenRead(oldPath);
			KeyboardMapping = (Dictionary<InputAction, KeyCode>)binaryFormatter.Deserialize(fileStream2);
			fileStream2.Close();
			File.Delete(oldPath);
			Save(fileName);
			return;
		}
		if (File.Exists(filePath))
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<InputMapping>));
			List<InputMapping> inputMappings;

			using (StreamReader reader = new StreamReader(filePath))
			{
				inputMappings = (List<InputMapping>)serializer.Deserialize(reader);
			}
			KeyboardMapping = new Dictionary<InputAction, KeyCode>();
			foreach (var mapping in inputMappings)
			{
				KeyboardMapping.Add(mapping.Action, mapping.KeyCode);
			}
		}
		else
		{
			SetDefaults();
			Save(fileName);
			Debug.Log("Controls_" + fileName + " doesn't exist. Loading defaults...");
		}
	}
	
	public void SetDefaults()
	{
		Modifiy(InputAction.MoveForward, KeyCode.W);
		Modifiy(InputAction.MoveBackward, KeyCode.S);
		Modifiy(InputAction.MoveLeft, KeyCode.A);
		Modifiy(InputAction.MoveRight, KeyCode.D);
		Modifiy(InputAction.Run, KeyCode.LeftShift);
		Modifiy(InputAction.LookBehindJump, KeyCode.Space);
		Modifiy(InputAction.PauseOrCancel, KeyCode.Escape);
		Modifiy(InputAction.Interact, KeyCode.E);
		Modifiy(InputAction.UseItem, KeyCode.Q);
		Modifiy(InputAction.Slot0, KeyCode.Alpha1);
		Modifiy(InputAction.Slot1, KeyCode.Alpha2);
		Modifiy(InputAction.Slot2, KeyCode.Alpha3);
		Debug.Log("Settings reset.");
	}
}

public class InputMapping
{
	public InputAction Action { get; set; }
	public KeyCode KeyCode { get; set; }
}