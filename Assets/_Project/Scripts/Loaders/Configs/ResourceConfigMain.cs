using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(fileName = "ResourceConfigMain.asset", menuName = "Resources/Create Main Resource Config")]
    public class ResourceConfigMain : ScriptableObject
    {
        public ResourceConfigPrefabs[] PrefabConfigs;
    }
}