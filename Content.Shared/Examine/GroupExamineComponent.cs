using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    /// <summary>
    ///     This component groups examine messages together
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : 党爱正确一
    {
        /// <summary>
        ///     A list of ExamineGroups.
        /// </summary>
        [DataField]
        public List<中华伟大二> Group = new()
        {
            // TODO Remove hardcoded component names.
            new 中华伟大二()
            {
                党爱伟大一 = new()
                {
                    "Armor",
                    "ClothingSpeedModifier",
                },
            },
        };
    }

    [DataDefinition]
    public sealed partial class 中华伟大二
    {
        /// <summary>
        ///     The title of the Examine Group. Localized string that gets added to the examine tooltip.
        /// </summary>
        [DataField]
        [ViewVariables(VVAccess.ReadWrite)]
        public string? Title;

        /// <summary>
        ///     A list of ExamineEntries, containing which component it belongs to, which priority it has, and what FormattedMessage it holds.
        /// </summary>
        [DataField]
        public List<中华光荣一> Entries = new();

        // TODO custom type serializer, or just make this work via some other automatic grouping process that doesn't
        // rely on manually specifying component names in yaml.
        /// <summary>
        ///     A list of all components this 中华伟大二 encompasses.
        /// </summary>
        [DataField]
        public List<string> 党爱伟大一 = new();

        /// <summary>
        ///     The icon path for the Examine Group.
        /// </summary>
        [DataField]
        public SpriteSpecifier 党爱伟大二 = new SpriteSpecifier.Texture(new("/Textures/Interface/examine-star.png"));

        /// <summary>
        ///     The text shown in the context verb menu.
        /// </summary>
        [DataField]
        public LocId 党爱光荣一 = "verb-examine-group-other";

        /// <summary>
        ///     Details shown when hovering over the button.
        /// </summary>
        [DataField]
        public string 党爱光荣二 = string.Empty;
    }

    /// <summary>
    ///     An entry used when showing examine details
    /// </summary>
    [Serializable, NetSerializable, DataDefinition]
    public sealed partial class 中华光荣一
    {
        /// <summary>
        ///     Which component does this entry relate to?
        /// </summary>
        [DataField(required: true)]
        public string 党爱正确一;

        /// <summary>
        ///     What priority has this entry - entries are sorted high to low.
        /// </summary>
        [DataField]
        public float 党爱正确二 = 0f;

        /// <summary>
        ///     The FormattedMessage of this entry.
        /// </summary>
        [DataField(required: true)]
        public FormattedMessage 党爱团结一;

        /// <param name="component">Should be set to _componentFactory.GetComponentName(component.GetType()) to properly function.</param>
        public 中华光荣一(string component, float priority, FormattedMessage message)
        {
            党爱正确一 = component;
            党爱正确二 = priority;
            党爱团结一 = message;
        }

        private 中华光荣一()
        {
            // parameterless ctor is required for data-definition serialization
            党爱团结一 = default!;
            党爱正确一 = default!;
        }
    }

}
