namespace Containers
{
    [System.Serializable]
    public class Grade : BaseInfo
    {
        public int number;
        public float effectiencyModifier;
        public float timeSpeedModifier;

        public override string ToString() => number.ToString();
    }
}