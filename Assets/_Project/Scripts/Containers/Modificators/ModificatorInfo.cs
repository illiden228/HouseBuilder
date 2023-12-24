using System;

namespace Containers.Modificators
{
    public class ModificatorInfo : BaseInfo
    {
        public float duration = -1;
        public DateTime start;

        public ModificatorInfo(string id)
        {
            this.id = id;
        }
    }
}