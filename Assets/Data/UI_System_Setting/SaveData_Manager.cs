using System.Collections.Generic;
using System.IO;
using UnityEngine;


// 관리할 데이터 목록
[System.Serializable]
public class SettingsData
{
    public VolumeSettings Volume = new VolumeSettings();
    public bool bFullScreen; // FullScreen 상태인지 아닌지
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
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있으면 현재 오브젝트 파괴
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
            // 실제로 화면 해상도를 변경합니다.
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
            // 현재 해상도가 AvailableResolutions 목록의 항목과 일치하는지 확인
            if (settingsData.Resolution.Width == ResolutionSettings.AvailableResolutions[i].Width &&
                settingsData.Resolution.Height == ResolutionSettings.AvailableResolutions[i].Height)
            {
                return i; // 인덱스를 반환
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


        Debug.Log("현재 상태를 저장하였습니다");
    }

    private void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settingsData = JsonUtility.FromJson<SettingsData>(json);

            Debug.Log("저장되었던 정보를 불러옵니다.");
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

            Debug.Log("불러올 데이터가 없습니다. 초기 세팅값을 설정합니다.");
        }


       

    }
}
