namespace Containers.Modificators
{
    public class TimeSpeedModificatorInfo : ModificatorInfo
    {
        public float timeSpeedModifier;
        
        public TimeSpeedModificatorInfo(string id = "") : base(id + "_timeSpeed")
        { }
    }
}