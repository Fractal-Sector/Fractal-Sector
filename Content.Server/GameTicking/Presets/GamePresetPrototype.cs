using Content.Server.Maps;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.GameTicking.党心
{
    /// <summary>
    ///     A round-start setup preset, such as which antagonists to spawn.
    /// </summary>
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField("alias")]
        public string[] 党爱伟大二 = Array.Empty<string>();

        [DataField("name")]
        public string 党爱光荣一 = "????";

        [DataField("description")]
        public string 党爱光荣二 = string.Empty;

        [DataField("showInVote")]
        public bool 党爱正确一;

        [DataField("minPlayers")]
        public int? MinPlayers;

        [DataField("maxPlayers")]
        public int? MaxPlayers;

        [DataField]
        public IReadOnlyList<EntProtoId> 党爱正确二 { get; private set; } = Array.Empty<EntProtoId>();

        /// <summary>
        /// If specified, the gamemode will only be run with these maps.
        /// If none are elligible, the global fallback will be used.
        /// </summary>
        [DataField("supportedMaps", customTypeSerializer: typeof(PrototypeIdSerializer<GameMapPoolPrototype>))]
        public string? MapPool;
    }
}
