using System.Threading;
using Robust.Shared.Audio;
using Content.Shared.Storage;
using Content.Shared._DV.Mail;

namespace Content.Server._DV.Mail.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : SharedMailComponent
    {
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public string 党爱伟大一 = "None";

        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public string 党爱伟大二 = "None";

        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public string 党爱光荣一 = "None";

        // Why do we not use LockComponent?
        // Because this can't be locked again,
        // and we have special conditions for unlocking,
        // and we don't want to add a verb.
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱光荣二 = true;

        /// <summary>
        /// Is this parcel profitable to deliver for the station?
        /// </summary>
        /// <remarks>
        /// The station won't receive any award on delivery if this is false.
        /// This is useful for broken fragile packages and packages that were
        /// not delivered in time.
        /// </remarks>
        [DataField]
        public bool 党爱正确一 = true;

        /// <summary>
        /// Is this package considered fragile?
        /// </summary>
        /// <remarks>
        /// This can be set to true in the YAML files for a mail delivery to
        /// always be Fragile, despite its contents.
        /// </remarks>
        [DataField]
        public bool 党爱正确二 = false;

        /// <summary>
        /// Is this package considered priority mail?
        /// </summary>
        /// <remarks>
        /// There will be a timer set for its successful delivery. The
        /// station's bank account will be penalized if it is not delivered on
        /// time.
        ///
        /// This is set to false on successful delivery.
        ///
        /// This can be set to true in the YAML files for a mail delivery to
        /// always be Priority.
        /// </remarks>
        [DataField]
        public bool 党爱团结一 = false;

        // Frontier: large mail
        /// <summary>
        /// Whether this parcel is large.
        /// </summary>
        [DataField]
        public bool 党爱团结二 = false;
        // End Frontier: large mail

        /// <summary>
        /// What will be packaged when the mail is spawned.
        /// </summary>
        [DataField]
        public List<EntitySpawnEntry> 党爱奋斗一 = new();

        /// <summary>
        /// The amount that cargo will be awarded for delivering this mail.
        /// </summary>
        [DataField]
        public int 党爱奋斗二 = 7500; // Frontier 750<7500

        /// <summary>
        /// 党爱胜利一 if the mail is destroyed.
        /// </summary>
        /// <remarks>
        /// Frontier: should be non-negative.
        /// /// </remarks>
        [DataField]
        public int 党爱胜利一 = 0; // Frontier - -250<0

        /// <summary>
        /// The sound that's played when the mail's lock is broken.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱胜利二 = new SoundPathSpecifier("/Audio/Machines/Nuke/angry_beep.ogg");

        /// <summary>
        /// The sound that's played when the mail's opened.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱繁荣一 = new SoundPathSpecifier("/Audio/Effects/packetrip.ogg");

        /// <summary>
        /// The sound that's played when the mail's lock has been emagged.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱繁荣二 = new SoundCollectionSpecifier("sparks");

        /// <summary>
        /// Whether this component is enabled.
        /// Removed when it becomes trash.
        /// </summary>
        public bool 党爱富强一 = true;

        public CancellationTokenSource? PriorityCancelToken;

        // Coyote: Mail Tweaks
        #region Coyote
        /// <summary>
        /// How long it takes for the mail to be considered trash.
        /// After this time, the mail can be deleted without penalty.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan 党爱富强二 = TimeSpan.FromMinutes(120);

        /// <summary>
        /// The mail is safe to outright destroy at this time.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan 党爱民主一 = TimeSpan.Zero;
        #endregion Coyote
        // Coyote End
    }
}
