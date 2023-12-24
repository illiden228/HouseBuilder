using System.Drawing;

namespace Containers.Modificators
{
    public class EffectiencyModificatorInfo : ModificatorInfo
    {
        public int incomeMoneyModifier;
        public int incomeWorkModifier;

        public EffectiencyModificatorInfo(string id = "")  : base(id + "_timeSpeed")
        { }
    }
}