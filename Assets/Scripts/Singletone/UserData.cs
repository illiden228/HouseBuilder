using UnityEngine;

public class UserData : MonoBehaviour
{
    [System.Serializable]
    public class ProgressData
    {
        public int countFloors = 0;
    }

    [System.Serializable]
    public class SettingsData
    {
        public bool soundOn = true;
        public bool musicOn = true;
        public bool vibrationOn = true;
    }

    private ProgressData _progressData = new ProgressData();
    private SettingsData _settingsData = new SettingsData();

    private const string ProgressKey = "ProgressKey";
    private const string SettingsKey = "SettingsKey";

    private IStorageService _storageService;

    private bool inited = false;

    #region data managment

    private static UserData instance;
    public static UserData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UserData();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        Init();
    }

    public void Init()
    {
        if (inited)
            return;

        inited = true;

        _storageService = new JsonToFileStorageService();

        InitFirstData();

        _storageService.LoadAndPopulate(ProgressKey, _progressData);
        _storageService.LoadAndPopulate(SettingsKey, _settingsData);
    }

    private void InitFirstData()
    {
        _settingsData.musicOn = true;
        _settingsData.soundOn = true;
        _settingsData.vibrationOn = true;        
    }

    public void SaveProgressData()
    {
        _storageService.SaveAsync(ProgressKey, _progressData);
    }

    public void SaveSettings()
    {
        _storageService.SaveAsync(SettingsKey, _settingsData);
    }

    public void ResetAllData()
    {
        _progressData = new ProgressData();
        _settingsData = new SettingsData();

        InitFirstData();
        SaveProgressData();
        SaveSettings();        
    }


    #endregion

    #region sound and vibro       
    public bool SoundOn
    {
        get { return _settingsData.soundOn; }
        set
        {
            if (_settingsData.soundOn == value)
                return;

            _settingsData.soundOn = value;
            SaveSettings();

            //SoundManager.Instance.TurnOnOffAudio(value);
        }
    }

    public bool VibrationOn
    {
        get { return _settingsData.vibrationOn; }
        set
        {
            if (_settingsData.vibrationOn == value)
                return;

            _settingsData.vibrationOn = value;
            SaveSettings();
        }
    }

    #endregion

    public int CountFloors
    {
        get { return _progressData.countFloors; }
        set
        {
            if (_progressData.countFloors == value)
                return;

            _progressData.countFloors = value;
            _progressData.countFloors = Mathf.Clamp(value, 0, _progressData.countFloors);

            SaveProgressData();
        }
    }
}
