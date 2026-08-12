using Content.Shared.Storage;

namespace Content.Server.党爱伟大一.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("tools")] public List<EntitySpawnEntry> 党爱伟大一 = new();
        public List<EntityUid> 党爱伟大二 = new();
        public List<string> 党爱光荣一 = new();
    }
}
