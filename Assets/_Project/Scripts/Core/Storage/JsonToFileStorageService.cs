using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Containers.Data;
using UnityEngine;

public class JsonToFileStorageService : IStorageService
{
    public struct Ctx
    {
    }

    private readonly Ctx _ctx;

    public JsonToFileStorageService(Ctx ctx)
    {
        _ctx = ctx;
    }

    public void ClearData(string key)
    {
        string path = BuildPath(key);

        if (!File.Exists(path))
            return;

        File.Delete(path);
    }

    public ConfigData LoadConfig(string key)
    {
        // TODO: закинуть конфиг в какое нибудь другое место, он будет с remote config грузиться
        //string path = Path.Combine(Application, key);
        string json = Resources.Load<TextAsset>(key).text;
        ConfigData result = JsonConvert.DeserializeObject<ConfigData>(json);
// #if UNITY_ANDROID
//         result = Load<ConfigData>(path);
// #else
//         UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
//         Debug.Log("sed request for " + path);        
//         www.SendWebRequest();
//         while (!www.isDone) {}
//         string jsonString = www.downloadHandler.text;
//         Debug.LogError("json: " + jsonString); 
//         result = JsonConvert.DeserializeObject<ConfigData>(jsonString);
//         Debug.LogError("DeserializeObject: " + result); 
// #endif

        return result;
    }

    public void Load<T>(string key, Action<T> callBack) where T : class
    {
        string path = BuildPath(key);

        var data = Load<T>(path);
        callBack?.Invoke(data);
    }

    private T Load<T>(string path) where T : class
    {
        if (!File.Exists(path))
            return null;

        using (var fileStream = new StreamReader(path))
        {
            var json = fileStream.ReadToEnd();
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        return null;
    }

    public void LoadAndPopulate<T>(string key, T dataToPopulate, Action callBack = null)
    {
        string path = BuildPath(key);

        if (!File.Exists(path))
            return;

        using (var fileStream = new StreamReader(path))
        {
            var json = fileStream.ReadToEnd();
            if (!string.IsNullOrEmpty(json))
            {
                JsonConvert.PopulateObject(json, dataToPopulate);
                callBack?.Invoke();
            }
        }
    }

    public void Save(string key, object data, Action<bool> callBack = null)
    {
        string path = BuildPath(key);
        string json = JsonConvert.SerializeObject(data);

        using (var fileStream = new StreamWriter(path))
        {
            fileStream.Write(json);
        }

        callBack?.Invoke(true);
    }

    public async Task SaveAsync(string key, object data, Action<bool> callBack = null)
    {
        string path = BuildPath(key);
        string json = JsonConvert.SerializeObject(data);

        using (var fileStream = new StreamWriter(path))
        {
            await fileStream.WriteAsync(json);
        }

        callBack?.Invoke(true);
    }

    private string BuildPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key);
    }
}