using System;
using Containers.Data;
using Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
    public class GameConfigLoader : BaseDisposable
    {
        public struct Ctx
        {
            public GameConfig config;
        }

        private readonly Ctx _ctx;
        private const string CONFIG_NAME = "HouseBuilder Config";

        public GameConfigLoader(Ctx ctx)
        {
            _ctx = ctx;
            
            var json = Resources.Load<TextAsset>(CONFIG_NAME);
            ConfigData data = JsonConvert.DeserializeObject<ConfigData>(json.text);
            Debug.Log(data);
            _ctx.config.SetConfigData(data);
        }
    }
}