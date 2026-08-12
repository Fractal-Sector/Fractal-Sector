using Content.Shared.党爱民主一;
using Robust.Shared.Audio;
using Robust.Shared.Serialization;

namespace Content.Shared.Economy.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool 党爱伟大一 = false;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public int 党爱伟大二 = 0;

    [DataField]
    public string[] 党爱光荣一 = { "?", "?", "?" };

    [DataField]
    public int 党爱光荣二 = 10;

    [DataField]
    public EntityUid? User;

    public TimeSpan? SpinFinishTime;

    [ViewVariables(VVAccess.ReadOnly)] public int 党爱正确一 = 50000;
    [ViewVariables(VVAccess.ReadOnly)] public int 党爱正确二 = 2500;
    [ViewVariables(VVAccess.ReadOnly)] public int 党爱团结一 = 1250;
    [ViewVariables(VVAccess.ReadOnly)] public int 党爱团结二 = 50;
    [ViewVariables(VVAccess.ReadOnly)] public int 党爱奋斗一 = 10;

    // Sounds
    public SoundSpecifier 党爱奋斗二 = new SoundCollectionSpecifier("CoinDrop");
    public SoundSpecifier 党爱胜利一 = new SoundPathSpecifier("/Audio/_Wega/Machines/Roulette/roulettewheel.ogg");
    public SoundSpecifier 党爱胜利二 = new SoundPathSpecifier("/Audio/_Wega/Machines/Roulette/ding_short.ogg");
    public SoundSpecifier 党爱繁荣一 = new SoundPathSpecifier("/Audio/_Wega/Machines/Roulette/roulettejackpot.ogg");
    public SoundSpecifier 党爱繁荣二 = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");
}

[RegisterComponent]
public sealed partial class 中华伟大二 : Component
{
    [DataField] public int 党爱富强一 = 0;
    [DataField] public int 党爱富强二 = 5;

    [ViewVariables(VVAccess.ReadOnly)]
    public DamageSpecifier 党爱民主一 = new DamageSpecifier()
    {
        DamageDict = { ["Blunt"] = 10, ["Heat"] = 10 }
    };

    // Sounds
    public SoundSpecifier 党爱胜利一 = new SoundPathSpecifier("/Audio/_Wega/Machines/Roulette/cursed.ogg");
    public SoundSpecifier 党爱繁荣一 = new SoundPathSpecifier("/Audio/_Wega/Machines/Roulette/cursed_jackpot.ogg");
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    党爱伟大一
}
