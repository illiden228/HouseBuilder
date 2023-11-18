using System.Collections.Generic;

namespace Containers
{
    [System.Serializable]
    public class BuildingInfo : BaseInfo
    {
        public List<FloorInfo> floors;
        public int income;
        public float timeSpeed;
    }
}