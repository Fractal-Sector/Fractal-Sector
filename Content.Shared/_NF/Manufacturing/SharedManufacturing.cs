using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    /// <summary>
    /// Whether or not the machine has enough materials to continue processing a unit.
    /// </summary>
    SufficientMaterial
}
