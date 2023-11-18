using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "ResourceConfigMain.asset", menuName = "Resources/Create Main Resource Config")]
    public class ResourceConfigMain : ScriptableObject
    {
        public ResourceConfigPrefabs[] PrefabConfigs;
    }
}