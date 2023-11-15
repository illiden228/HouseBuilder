using System;
using System.Threading.Tasks;

public interface IStorageService
{
    void Save(string key, object data, Action<bool> callBack = null);
    Task SaveAsync(string key, object data, Action<bool> callBack = null);
    void Load<T>(string key, Action<T> callBack);
    void LoadAndPopulate<T>(string key, T dataToPopulate, Action callBack = null);
    void ClearData(string key);
}
