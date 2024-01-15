using System.Collections.Generic;

namespace Containers
{
    [System.Serializable]
    public class BuildingInfo : BaseInfo
    {
        public List<FloorInfo> floors;
        public int income;
        public float timeSpeed;
        public int minReward = 20; //Temp если здание сломано
        public int maxReward = 100; //Если здание 90% процентов ровное
        public int preceptBuildingQuality = 0;
    }
}