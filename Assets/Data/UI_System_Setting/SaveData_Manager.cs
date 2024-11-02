using System.Collections.Generic;
using System.IO;
using UnityEngine;


// ������ ������ ���
[System.Serializable]
public class SettingsData
{
    public VolumeSettings Volume = new VolumeSettings();
    public bool bFullScreen; // FullScreen �������� �ƴ���
    public ResolutionSettings Resolution = new ResolutionSettings();
}

[System.Serializable]
public class VolumeSettings
{
    public float Master;
    public float BGM;
    public float Effect;
}

[System.Serializable]
public class ResolutionSettings
{
    public int Width;
    public int Height;

    public static List<ResolutionSettings> AvailableResolutions = new List<ResolutionSettings>
    {
        new ResolutionSettings { Width = 720, Height = 480 },
        new ResolutionSettings { Width = 1280, Height = 720 },
        new ResolutionSettings { Width = 1920, Height = 1080 },
        new ResolutionSettings { Width = 2560, Height = 1440 }
    };

    public string GetResolutionName()
    {
        return $"{Width}x{Height}";
    }
}
//===================================================================================================


public class SaveData_Manager : MonoBehaviour
{
    public static SaveData_Manager Instance { get; private set; }

    private SettingsData settingsData = new SettingsData();
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϴ� �ν��Ͻ��� ������ ���� ������Ʈ �ı�
        }

        filePath = Path.Combine(Application.persistentDataPath, "UISetting.json");
        LoadSettings();
    }


    //===================================================================================================
    // Volume setters
    public void SetMasterVolume(float value)
    {
        settingsData.Volume.Master = value;
        SaveSettings();
    }

    public void SetBGMVolume(float value)
    {
        settingsData.Volume.BGM = value;
        SaveSettings();
    }

    public void SetEffectVolume(float value)
    {
        settingsData.Volume.Effect = value;
        SaveSettings();
    }
    //===================================================================================================
    // Volume getters
    public float GetMasterVolume()
    {
        return settingsData.Volume.Master;
    }

    public float GetBGMVolume()
    {
        return settingsData.Volume.BGM;
    }

    public float GetEffectVolume()
    {
        return settingsData.Volume.Effect;
    }

    //===================================================================================================
    // FullScreen setters and getters
    public void SetFullScreen(bool isFullScreen)
    {
        settingsData.bFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
        SaveSettings();
    }

    public bool GetBoolFullScreen()
    {
        return settingsData.bFullScreen;
    }
    //===================================================================================================
    // Resolution setters and getters
    public void SetResolution(int index)
    {
        if (index >= 0 && index < ResolutionSettings.AvailableResolutions.Count)
        {
            settingsData.Resolution = ResolutionSettings.AvailableResolutions[index];
            // ������ ȭ�� �ػ󵵸� �����մϴ�.
            Screen.SetResolution(settingsData.Resolution.Width, settingsData.Resolution.Height, settingsData.bFullScreen);
            SaveSettings();
        }
        else
        {
            Debug.LogWarning("Invalid resolution index.");
        }
    }

    public ResolutionSettings GetResolution()
    {
        return settingsData.Resolution;
    }
    public int GetResolutionIndex()
    {
        for (int i = 0; i < ResolutionSettings.AvailableResolutions.Count; i++)
        {
            // ���� �ػ󵵰� AvailableResolutions ����� �׸�� ��ġ�ϴ��� Ȯ��
            if (settingsData.Resolution.Width == ResolutionSettings.AvailableResolutions[i].Width &&
                settingsData.Resolution.Height == ResolutionSettings.AvailableResolutions[i].Height)
            {
                return i; // �ε����� ��ȯ
            }
        }
        Debug.LogWarning("Current resolution not found in available resolutions.");
        return -1;
    }
    //===================================================================================================
    // Save and load methods
    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(settingsData, true);
        File.WriteAllText(filePath, json);


        Debug.Log("���� ���¸� �����Ͽ����ϴ�");
    }

    private void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settingsData = JsonUtility.FromJson<SettingsData>(json);

            Debug.Log("����Ǿ��� ������ �ҷ��ɴϴ�.");
        }
        else
        {
            // Default values
            settingsData.Volume.Master = 1.0f;
            settingsData.Volume.BGM = 0.5f;
            settingsData.Volume.Effect = 0.5f;
            settingsData.bFullScreen = true;
            settingsData.Resolution = ResolutionSettings.AvailableResolutions[3]; 
            SaveSettings();

            Debug.Log("�ҷ��� �����Ͱ� �����ϴ�. �ʱ� ���ð��� �����մϴ�.");
        }


       

    }
}
