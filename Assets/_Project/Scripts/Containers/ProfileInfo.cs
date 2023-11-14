using System.Collections.Generic;
using Containers.Modificators;

namespace Containers
{
    public class ProfileInfo : BaseInfo
    {
        public int moneys;
        public List<WorkerInfo> workers;
        public List<BuildingInfo> buildings;
        public List<Modificator> modificators;
    }
}