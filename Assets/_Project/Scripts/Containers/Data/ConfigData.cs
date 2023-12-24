using System.Collections.Generic;
using _Project.Scripts.Containers.Data;

namespace Containers.Data
{
    public class ConfigData
    {
        public MainSettings Main;
        public List<WorkerData> workerInfos;
        public List<LevelUpConstData> levelUpCosts;
        public List<BuildingData> buildingInfos;
        public List<FloorData> floorInfos;
    }
}