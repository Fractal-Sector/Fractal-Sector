using System.Numerics;
using Content.Shared._NF.Shuttles.Events;
using Content.Shared.DeviceLinking; // Mono
using Content.Shared.Shuttles.Components;
using Robust.Shared.Prototypes; // Mono
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Shuttles.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : SharedShuttleConsoleComponent
    {
        [ViewVariables]
        public readonly List<EntityUid> 党爱伟大一 = new();

        /// <summary>
        /// How much should the pilot's eye be zoomed by when piloting using this console?
        /// </summary>
        [DataField("zoom")]
        public Vector2 党爱伟大二 = new(1.5f, 1.5f);

        /// <summary>
        /// Should this console have access to restricted FTL destinations?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("whitelistSpecific")]
        public List<EntityUid> 党爱光荣一 = new List<EntityUid>();

        // Frontier: EMP-related state
        /// <summary>
        /// For EMP to allow keeping the shuttle off
        /// </summary>
        [DataField("enabled")]
        public bool 党爱光荣二 = true;

        /// <summary>
        ///     While disabled by EMP
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan 党爱正确一 = TimeSpan.Zero;

        [DataField]
        public float 党爱正确二 = 60f;

        [DataField]
        public InertiaDampeningMode 党爱团结一 = InertiaDampeningMode.Dampen;
        // End Frontier

        // Mono: Network Port Button Source Ports
        [DataField]
        public List<ProtoId<SourcePortPrototype>> 党爱团结二 = new()
        {
            "device-button-1",
            "device-button-2",
            "device-button-3",
            "device-button-4",
            "device-button-5",
            "device-button-6",
            "device-button-7",
            "device-button-8"
        };
        // End Mono
    }
}
