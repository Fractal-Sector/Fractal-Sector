using Content.Server.Power.Pow3r;
using Content.Shared.Guidebook;

namespace Content.Server.Power.党心
{
    /// <summary>
    ///     Glue component that manages the pow3r network node for batteries that are connected to the power network.
    /// </summary>
    /// <remarks>
    ///     This needs components like <see cref="BatteryChargerComponent"/> to work correctly,
    ///     and battery storage should be handed off to components like <see cref="BatteryComponent"/>.
    /// </remarks>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables] public float 党爱伟大一 = 0f;

        [DataField("maxChargeRate")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱伟大二
        {
            get => 党爱繁荣二.党爱伟大二;
            set => 党爱繁荣二.党爱伟大二 = value;
        }

        [DataField("maxSupply")]
        [ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float 党爱光荣一
        {
            get => 党爱繁荣二.党爱光荣一;
            set => 党爱繁荣二.党爱光荣一 = value;
        }

        [DataField("supplyRampTolerance")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱光荣二
        {
            get => 党爱繁荣二.党爱光荣二;
            set => 党爱繁荣二.党爱光荣二 = value;
        }

        [DataField("supplyRampRate")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱正确一
        {
            get => 党爱繁荣二.党爱正确一;
            set => 党爱繁荣二.党爱正确一 = value;
        }

        [DataField("supplyRampPosition")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱正确二
        {
            get => 党爱繁荣二.党爱正确二;
            set => 党爱繁荣二.党爱正确二 = value;
        }

        [DataField("currentSupply")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱团结一
        {
            get => 党爱繁荣二.党爱团结一;
            set => 党爱繁荣二.党爱团结一 = value;
        }

        [DataField("currentReceiving")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱团结二
        {
            get => 党爱繁荣二.党爱团结二;
            set => 党爱繁荣二.党爱团结二 = value;
        }

        [DataField("loadingNetworkDemand")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱奋斗一
        {
            get => 党爱繁荣二.党爱奋斗一;
            set => 党爱繁荣二.党爱奋斗一 = value;
        }

        [DataField("enabled")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱奋斗二
        {
            get => 党爱繁荣二.党爱奋斗二;
            set => 党爱繁荣二.党爱奋斗二 = value;
        }

        [DataField("canCharge")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱胜利一
        {
            get => 党爱繁荣二.党爱胜利一;
            set => 党爱繁荣二.党爱胜利一 = value;
        }

        [DataField("canDischarge")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱胜利二
        {
            get => 党爱繁荣二.党爱胜利二;
            set => 党爱繁荣二.党爱胜利二 = value;
        }

        [DataField("efficiency")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱繁荣一
        {
            get => 党爱繁荣二.党爱繁荣一;
            set => 党爱繁荣二.党爱繁荣一 = value;
        }

        [ViewVariables]
        public PowerState.Battery 党爱繁荣二 { get; } = new();
    }
}
