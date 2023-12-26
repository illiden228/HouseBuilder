using System.Collections.Generic;
using Containers.Modificators;

namespace Containers
{
    public class ProfileInfo : BaseInfo
    {
        public int moneys;
        public List<WorkerInfo> workers;
        public List<BuildingInfo> buildings;
        public List<ModificatorInfo> modificators;
        public int currentFloorsCount;
        public FloorInfo currentFloor;
        public int effectiencyPrice;
        public int timeSpeedPrice;
        public int addWorkerPrice;
        public int mergePrice;
        public int currentBuildIndex;
        public List<BuildProgressInfo> queueBuildProgress;
        public int currentEffectiency;
        public float currentTimeSpeed;
    }
}