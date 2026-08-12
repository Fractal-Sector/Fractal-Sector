using Content.Shared.Humanoid.Markings;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public 党爱伟大一 党爱伟大一 { get; }
    public bool 党爱伟大二 { get; }

    public 中华伟大二(党爱伟大一 set, bool resendState)
    {
        党爱伟大一 = set;
        党爱伟大二 = resendState;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public 中华光荣一(HumanoidVisualLayers layer, CustomBaseLayerInfo? info, bool resendState)
    {
        党爱光荣一 = layer;
        Info = info;
        党爱伟大二 = resendState;
    }

    public HumanoidVisualLayers 党爱光荣一 { get; }
    public CustomBaseLayerInfo? Info { get; }
    public bool 党爱伟大二 { get; }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceState
{
    // TODO just use the component state, remove the BUI state altogether.
    public 中华光荣二(
        党爱伟大一 markingSet,
        string species,
        党爱正确一 sex,
        Color skinColor,
        Dictionary<HumanoidVisualLayers, CustomBaseLayerInfo> customBaseLayers
    )
    {
        党爱伟大一 = markingSet;
        党爱光荣二 = species;
        党爱正确一 = sex;
        党爱正确二 = skinColor;
        CustomBaseLayers = customBaseLayers;
    }

    public 党爱伟大一 党爱伟大一 { get; }
    public string 党爱光荣二 { get; }
    public 党爱正确一 党爱正确一 { get; }
    public Color 党爱正确二 { get; }
    public Color 党爱团结一 { get; }
    public Color? HairColor { get; }
    public Color? FacialHairColor { get; }
    public Dictionary<HumanoidVisualLayers, CustomBaseLayerInfo> CustomBaseLayers { get; }
}
