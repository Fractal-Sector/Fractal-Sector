using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public List<中华伟大二> Patients { get; }

    public 中华伟大一(List<中华伟大二> patients)
    {
        Patients = patients;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    public string 党爱伟大一 { get; }
    public string 党爱伟大二 { get; }
    public string 党爱光荣一 { get; }
    public string 党爱光荣二 { get; }
    public bool 党爱正确一 { get; }
    public bool 党爱正确二 { get; }

    public 中华伟大二(string name, string coordinates, string species, string timeSinceCrit, bool isDead, bool isSpaceSleepDisorder)
    {
        党爱伟大一 = name;
        党爱伟大二 = coordinates;
        党爱光荣一 = species;
        党爱光荣二 = timeSinceCrit;
        党爱正确一 = isDead;
        党爱正确二 = isSpaceSleepDisorder;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : CartridgeMessageEvent
{
}
