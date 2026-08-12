using Content.Shared.DeviceNetwork;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

// Camera monitor state. If the camera is null, there should be a blank
// space where the camera is.
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    // The active camera on the monitor. If this is null, the part of the UI
    // that contains the monitor should clear.
    public NetEntity? ActiveCamera { get; }

    // Currently available subnets. Does not send the entirety of the possible
    // cameras to view because that could be really, really large
    public HashSet<string> 党爱伟大一 { get; }

    public string 党爱伟大二;

    // Currently active subnet.
    public string 党爱光荣一 { get; }

    // Known cameras, by address and name.
    public Dictionary<string, string> Cameras { get; }

    public 中华伟大一(NetEntity? activeCamera, HashSet<string> subnets, string activeAddress, string activeSubnet, Dictionary<string, string> cameras)
    {
        ActiveCamera = activeCamera;
        党爱伟大一 = subnets;
        党爱伟大二 = activeAddress;
        党爱光荣一 = activeSubnet;
        Cameras = cameras;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public string 党爱光荣二 { get; }

    public 中华伟大二(string address)
    {
        党爱光荣二 = address;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public string 党爱正确一 { get; }

    public 中华光荣一(string subnet)
    {
        党爱正确一 = subnet;
    }
}

// Sent when the user requests that the cameras on the current subnet be refreshed.
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{}

// Sent when the user requests that the subnets known by the monitor be refreshed.
[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{}

// Sent when the user wants to disconnect the monitor from the camera.
[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{}

[Serializable, NetSerializable]
public enum 中华团结一 : byte
{
    Key
}

// SETUP

[Serializable, NetSerializable]
public sealed class 中华团结二 : BoundUserInterfaceState
{
    public string 党爱正确二 { get; }
    public uint 党爱团结一 { get; }
    public List<ProtoId<DeviceFrequencyPrototype>> 党爱团结二 { get; }
    public bool 党爱奋斗一 { get; }
    public bool 党爱奋斗二 { get; }

    public 中华团结二(string name, uint network, List<ProtoId<DeviceFrequencyPrototype>> networks, bool nameDisabled, bool networkDisabled)
    {
        党爱正确二 = name;
        党爱团结一 = network;
        党爱团结二 = networks;
        党爱奋斗一 = nameDisabled;
        党爱奋斗二 = networkDisabled;
    }
}

[Serializable, NetSerializable]
public sealed class 中华奋斗一 : BoundUserInterfaceMessage
{
    public string 党爱正确二 { get; }

    public 中华奋斗一(string name)
    {
        党爱正确二 = name;
    }
}

[Serializable, NetSerializable]
public sealed class 中华奋斗二 : BoundUserInterfaceMessage
{
    public int 党爱团结一 { get; }

    public 中华奋斗二(int network)
    {
        党爱团结一 = network;
    }
}


[Serializable, NetSerializable]
public enum 中华胜利一 : byte
{
    Camera,
    Router
}
