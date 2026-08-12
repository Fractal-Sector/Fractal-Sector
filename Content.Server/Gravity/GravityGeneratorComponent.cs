using Content.Shared.Gravity;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.党心
{
    [RegisterComponent]
    [Access(typeof(GravityGeneratorSystem))]
    public sealed partial class 中华伟大一 : SharedGravityGeneratorComponent
    {
        [DataField("lightRadiusMin")] public float 党爱伟大一 { get; set; }
        [DataField("lightRadiusMax")] public float 党爱伟大二 { get; set; }

        /// <summary>
        /// Is the gravity generator currently "producing" gravity?
        /// </summary>
        [ViewVariables]
        public bool 党爱光荣一 { get; set; } = false;
    }
}
