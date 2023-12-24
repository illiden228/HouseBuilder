using System.Collections.Generic;
using _Project.Scripts.Containers.Data;

namespace Containers.Data
{
    public class ConfigData
    {
        public MainSettings main;
        public List<WorkerData> workerData;
        public List<LevelUpConstData> levelUpCosts;
        public List<BuildingData> buildingData;
        public List<FloorData> floorData;
    }
}