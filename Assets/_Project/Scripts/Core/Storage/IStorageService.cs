using System;
using System.Threading.Tasks;
using Containers.Data;
using UnityEngine;

public interface IStorageService
{
    void Save(string key, object data, Action<bool> callBack = null);
    Task SaveAsync(string key, object data, Action<bool> callBack = null);
    void Load<T>(string key, Action<T> callBack) where T : class;
    void LoadAndPopulate<T>(string key, T dataToPopulate, Action callBack = null);
    void ClearData(string key);
    ConfigData LoadConfig(string key);
}
