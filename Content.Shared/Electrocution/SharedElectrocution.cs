using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Sparks,
    HUD,
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    ShowSparks, // only shown when zapping someone, deactivated after a short time
    IsElectrified, // if the entity is electrified or not, used for the AI HUD
}
