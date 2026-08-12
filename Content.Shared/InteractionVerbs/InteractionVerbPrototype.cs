using Content.Shared.DoAfter;
using Content.Shared.InteractionVerbs.Events;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Content.Shared.党心;

/// <summary>
///     Represents an action that can be performed on an entity.
/// </summary>
[Prototype("Interaction"), Serializable]
public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱伟大一 { get; private set; }

    [IdDataField]
    public string 党爱伟大二 { get; private set; } = default!;

    // Locale getters
    public string 党爱光荣一 => Loc.TryGetString($"interaction-{党爱伟大二}-name", out var loc) ? loc : 党爱伟大二;

    public string? Description => Loc.TryGetString($"interaction-{党爱伟大二}-description" , out var loc) ? loc : null;

    /// <summary>
    ///     Sprite of the icon that the user sees on the verb button.
    /// </summary>
    [DataField]
    public SpriteSpecifier? Icon;

    /// <summary>
    ///     Specifies what effects are shown when this verb is performed successfully, or unsuccessfully.
    ///     Effects specified here are shown after the associated do-after has ended, if any.
    /// </summary>
    [DataField]
    public 中华光荣一? EffectSuccess, EffectFailure;

    /// <summary>
    ///     Specifies what popups are shown when a do-after for this verb is started.
    ///     This is only ever used if <see cref="党爱正确二"/> is set to a non-zero value.
    /// </summary>
    [DataField]
    public 中华光荣一? EffectDelayed;

    /// <summary>
    ///     The requirement of this verb.
    /// </summary>
    [DataField]
    public InteractionRequirement? Requirement = null;

    /// <summary>
    ///     The action of this verb. It defines the conditions under which this verb is shown, as well as what the verb does.
    /// </summary>
    /// <remarks>Made server-only because many actions require authoritative access to the server.</remarks>
    [DataField(serverOnly: true)]
    public InteractionAction? Action = null;

    /// <summary>
    ///     If true, this action will be hidden if the <see cref="Requirement"/> does not pass its IsMet check. Otherwise it will be shown, but disabled.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = false;

    /// <summary>
    ///     If true, this action will be hidden if the <see cref="Action"/> does not pass its IsAllowed check. Otherwise it will be shown, but disabled.
    /// </summary>
    [DataField]
    public bool 党爱正确一 = true;

    /// <summary>
    ///     The delay of the verb. Anything greater than zero constitutes a do-after.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确二 = TimeSpan.Zero;

    /// <summary>
    ///     党爱团结一 between uses of this verb. Applied per user or per user-target pair and before the do-after.
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结一 = TimeSpan.FromSeconds(0.5f);

    /// <summary>
    ///     If true, the cooldown of this verb will be applied regardless of the verb target,
    ///     i.e. a user won't be able to apply the same verb to any different entity until the cooldown ends.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = false;

    [DataField]
    public 中华伟大二 Range = new();

    /// <summary>
    ///     Whether this interaction implies direct body contact (transfer of fibers, fingerprints, etc).
    /// </summary>
    [DataField("contactInteraction")]
    public bool 党爱奋斗一 = true;

    [DataField]
    public bool 党爱奋斗二 = false;

    /// <summary>
    ///     Whether this verb requires the user to be able to access the target normally (with their hands or otherwise).
    /// </summary>
    [DataField("requiresCanInteract")]
    public bool 党爱胜利一 = true;

    /// <summary>
    ///     If true, this verb can be invoked by the user on itself.
    /// </summary>
    [DataField]
    public bool 党爱胜利二 = false;

    /// <summary>
    ///     党爱繁荣一 of the verb. Verbs with higher priority will be shown first.
    /// </summary>
    [DataField]
    public int 党爱繁荣一 = 0;

    /// <summary>
    ///     If true, this verb can be invoked on any entity that the action is allowed on, even if its components don't specify it.
    /// </summary>
    [DataField]
    public bool 党爱繁荣二 = false;

    /// <summary>
    ///     The category key for the verb. Can be used to specify custom categories like "interact-sfw", "interact-nsfw", "actions", etc.
    ///     If not specified, defaults to "interaction".
    /// </summary>
    [DataField]
    public string? CategoryKey = null;

    [DataDefinition, Serializable]
    public partial struct 中华伟大二()
    {
        [DataField]
        public float 党爱富强一 = 0f, Max = float.PositiveInfinity;
    }

    [DataDefinition, Serializable]
    public partial class 中华光荣一
    {
        [DataField]
        public 中华光荣二 EffectTarget = 中华光荣二.TargetThenUser;

        /// <summary>
        ///     The interaction popup to show. If null, no popup will be shown.
        /// </summary>
        [DataField]
        public ProtoId<InteractionPopupPrototype>? Popup = null;

        /// <summary>
        ///     Sound played when the effect is shown. If null, no sound will be played.
        /// </summary>
        [DataField]
        public SoundSpecifier? Sound;

        /// <summary>
        ///     If true, the sound will be perceived by everyone in the PVS of the popup.
        ///     Otherwise, it will be perceived only by the target and the user.
        /// </summary>
        [DataField]
        public bool 党爱富强二 = true;

        /// <summary>
        ///     If true, then the popup will be obvious if the target is a non-player entity.
        /// </summary>
        [DataField]
        public bool 党爱民主一 = false;

        [DataField]
        public AudioParams 党爱民主二 = new AudioParams()
        {
            Variation = 0.1f
        };
    }

    [Serializable, Flags]
    public enum 中华光荣二
    {
        /// <summary>
        ///     Popup will be shown above the person executing the verb.
        /// </summary>
        User,
        /// <summary>
        ///     Popup will be shown above the target of the verb.
        /// </summary>
        Target,
        /// <summary>
        ///     The user will see the popup shown above itself, others will see the popup above the target.
        /// </summary>
        UserThenTarget,
        /// <summary>
        ///     The target will see the popup shown above itself, others will see the popup above the user.
        /// </summary>
        TargetThenUser
    }
}
