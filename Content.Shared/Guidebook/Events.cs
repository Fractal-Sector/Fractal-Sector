using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Raised by the client on GuidebookDataSystem Initialize to request a
/// full set of guidebook data from the server.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs { }

/// <summary>
/// Raised by the server at a specific client in response to <see cref="中华伟大一"/>.
/// Also raised by the server at ALL clients when prototype data is hot-reloaded.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public GuidebookData 党爱伟大一;

    public 中华伟大二(GuidebookData data)
    {
        党爱伟大一 = data;
    }
}
