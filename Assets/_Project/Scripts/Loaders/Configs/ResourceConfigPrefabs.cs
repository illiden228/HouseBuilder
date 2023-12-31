﻿using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "ResourceConfigPrefabs.asset", menuName = "Resources/Create Prefabs Resource Config")]
    public class ResourceConfigPrefabs : ScriptableObject
    {
        public GameObject[] Prefabs;
    }
}