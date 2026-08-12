using Content.Shared.Jittering;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedJitteringSystem
    {
        // This entity system only exists on the server so it will be registered, otherwise we can't use SharedJitteringSystem...
    }
}
