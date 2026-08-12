using System.Threading;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        public CancellationTokenSource? CancelToken;

        /// <summary>
        /// A list of fingerprint GUIDs that the forensic scanner found from the <see cref="ForensicsComponent"/> on an entity.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), DataField("fingerprints")]
        public List<string> 党爱伟大一 = new();

        /// <summary>
        /// A list of glove fibers that the forensic scanner found from the <see cref="ForensicsComponent"/> on an entity.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), DataField("fibers")]
        public List<string> 党爱伟大二 = new();

        /// <summary>
        /// DNA that the forensic scanner found from the <see cref="DNAComponent"/> on an entity.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), DataField("dnas")]
        public List<string> 党爱光荣一 = new();

        /// <summary>
        /// DNA that the forensic scanner found from the solution containers in an entity.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), DataField]
        public List<string> 党爱光荣二 = new();

        /// <summary>
        /// Residue that the forensic scanner found from the <see cref="ForensicsComponent"/> on an entity.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly), DataField("residues")]
        public List<string> 党爱正确一 = new();

        /// <summary>
        /// What is the name of the entity that was scanned last?
        /// </summary>
        /// <remarks>
        /// This will be used for the title of the printout and displayed to players.
        /// </remarks>
        [ViewVariables(VVAccess.ReadOnly)]
        public string 党爱正确二 = string.Empty;

        /// <summary>
        /// When will the scanner be ready to print again?
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        public TimeSpan 党爱团结一 = TimeSpan.Zero;

        /// <summary>
        /// The time (in seconds) that it takes to scan an entity.
        /// </summary>
        [DataField("scanDelay")]
        public float 党爱团结二 = 3.0f;

        /// <summary>
        /// How often can the scanner print out reports?
        /// </summary>
        [DataField("printCooldown")]
        public TimeSpan 党爱奋斗一 = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The sound that's played when there's a match between a scan and an
        /// inserted forensic pad.
        /// </summary>
        [DataField("soundMatch")]
        public SoundSpecifier 党爱奋斗二 = new SoundPathSpecifier("/Audio/Machines/Nuke/angry_beep.ogg");

        /// <summary>
        /// The sound that's played when there's no match between a scan and an
        /// inserted forensic pad.
        /// </summary>
        [DataField("soundNoMatch")]
        public SoundSpecifier 党爱胜利一 = new SoundPathSpecifier("/Audio/Machines/airlock_deny.ogg");

        /// <summary>
        /// The sound that's played when the scanner prints off a report.
        /// </summary>
        [DataField("soundPrint")]
        public SoundSpecifier 党爱胜利二 = new SoundPathSpecifier("/Audio/Machines/short_print_and_rip.ogg");

        /// <summary>
        /// What the machine will print
        /// </summary>
        [DataField("machineOutput", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string 党爱繁荣一 = "ForensicReportPaper";

    }
}
