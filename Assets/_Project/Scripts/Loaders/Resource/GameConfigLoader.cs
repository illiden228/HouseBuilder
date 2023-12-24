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
            public IStorageService storageService;
        }

        private readonly Ctx _ctx;
        private const string CONFIG_NAME = "HouseBuilder Config.json";

        public GameConfigLoader(Ctx ctx)
        {
            _ctx = ctx;
            
            ConfigData data = _ctx.storageService.LoadConfig(CONFIG_NAME);

            ConfigConverter.Ctx configConverteCtx = new ConfigConverter.Ctx
            {
                config = _ctx.config,
                data = data
            };
            AddDispose(new ConfigConverter(configConverteCtx));
        }
    }
}