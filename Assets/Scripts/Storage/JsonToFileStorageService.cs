using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class JsonToFileStorageService : IStorageService
{
    public void ClearData(string key)
    {
        string path = BuildPath(key);

        if (!File.Exists(path))
            return;

        File.Delete(path);
    }

    public void Load<T>(string key, Action<T> callBack)
    {
        string path = BuildPath(key);

        if (!File.Exists(path))
            return;

        using (var fileStream = new StreamReader(path))
        {
            var json = fileStream.ReadToEnd();
            if (!string.IsNullOrEmpty(json))
            {
                var data = JsonConvert.DeserializeObject<T>(json);
                callBack?.Invoke(data);
            }
        }
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

    private string BuildPath(string key) { return Path.Combine(Application.persistentDataPath, key); }
}