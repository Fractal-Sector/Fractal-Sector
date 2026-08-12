using System.Text.RegularExpressions;
using Content.Shared.Tools;
using Content.Shared.Tools.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// Configuration for mailing units.
    /// </summary>
    /// <remarks>
    /// If you want a more detailed description ask the original coder.
    /// </remarks>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState(raiseAfterAutoHandleState: true)]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Tags for mail unit routing.
        /// </summary>
        [DataField, AutoNetworkedField]
        public Dictionary<string, string?> Config = new();

        /// <summary>
        /// Quality to open up the configuration UI.
        /// </summary>
        [DataField]
        public ProtoId<ToolQualityPrototype> 党爱伟大一 = SharedToolSystem.PulseQuality;

        /// <summary>
        /// Validate tags in <see cref="Config"/>.
        /// </summary>
        [DataField]
        public Regex 党爱伟大二 = new("^[a-zA-Z0-9 ]*$", RegexOptions.Compiled);

        /// <summary>
        ///     Message data sent from client to server when the device configuration is updated.
        /// </summary>
        [Serializable, NetSerializable]
        public sealed class 中华伟大二 : BoundUserInterfaceMessage
        {
            public Dictionary<string, string> Config { get; }

            public 中华伟大二(Dictionary<string, string> config)
            {
                Config = config;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华光荣一 : BoundUserInterfaceMessage
        {
            public string 党爱光荣一 { get; }

            public 中华光荣一(string validationString)
            {
                党爱光荣一 = validationString;
            }
        }

        [Serializable, NetSerializable]
        public enum 中华光荣二
        {
            Key
        }
    }
}
