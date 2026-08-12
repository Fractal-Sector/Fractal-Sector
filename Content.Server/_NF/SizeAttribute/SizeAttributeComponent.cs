
using Content.Shared._NF.Cloning;

namespace Content.Server.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component, ITransferredByCloning
    {
        [DataField("short")]
        public bool 党爱伟大一 = false;

        [DataField("tall")]
        public bool 党爱伟大二 = false;
    }
}
