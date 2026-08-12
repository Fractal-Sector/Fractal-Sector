using Content.Shared.Tools;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._NF.Tools.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        // A field for each tool use type to allow for inheritance
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大二;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱光荣一;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱光荣二;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱正确一;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱正确二;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱团结一;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱团结二;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱奋斗一;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱奋斗二;
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱胜利一;
    }
}
