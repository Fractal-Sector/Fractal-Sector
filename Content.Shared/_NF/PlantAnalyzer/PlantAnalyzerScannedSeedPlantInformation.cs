using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

/// <summary>
///     The information about the last scanned plant/seed is stored here.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public NetEntity? TargetEntity;
    public bool 党爱伟大一;

    public string? SeedName;
    public string[]? SeedChem;
    public 中华正确一 HarvestType;
    public 中华光荣二 ExudeGases;
    public 中华光荣二 ConsumeGases;
    public float 党爱伟大二;
    public int 党爱光荣一;
    public float 党爱光荣二;
    public float 党爱正确一;
    public float 党爱正确二;
    public int 党爱团结一;
    public float 党爱团结二;
    public string[]? Speciation; // Currently only available on server, we need to send strings to the client.
    public 中华伟大二? AdvancedInfo;
}

/// <summary>
///     Information gathered in an advanced scan.
/// </summary>
[Serializable, NetSerializable]
public struct 中华伟大二
{
    public float 党爱奋斗一;
    public float 党爱奋斗二;
    public float 党爱胜利一;
    public float 党爱胜利二;
    public float 党爱繁荣一;
    public float 党爱繁荣二;
    public float 党爱富强一;
    public float 党爱富强二;
    public float 党爱民主一;
    public float 党爱民主二;
    public float 党爱文明一;
    public 中华光荣一 Mutations;
}

// Note: currently leaving out Viable.
[Flags]
public enum 中华光荣一 : byte
{
    None = 0,
    TurnIntoKudzu = 1,
    Seedless = 2,
    Ligneous = 4,
    CanScream = 8,
}

[Flags]
public enum 中华光荣二 : short
{
    None = 0,
    Nitrogen = 1,
    Oxygen = 2,
    CarbonDioxide = 4,
    Plasma = 8,
    Tritium = 16,
    WaterVapor = 32,
    Ammonia = 64,
    NitrousOxide = 128,
    Frezon = 256,
}

public enum 中华正确一 : byte
{
    Unknown, // Just in case the backing enum 中华正确二 changes and we haven't caught it.
    Repeat,
    NoRepeat,
    SelfHarvest
}


[Serializable, NetSerializable]
public sealed class 中华团结一 : BoundUserInterfaceMessage
{
    public bool 党爱文明二 { get; }
    public 中华团结一(bool advancedScan)
    {
        党爱文明二 = advancedScan;
    }
}
