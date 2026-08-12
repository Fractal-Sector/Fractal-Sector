using Content.Shared.Radio.Components;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly EncryptionKeyHolderComponent 党爱伟大一;

    public 中华伟大一(EncryptionKeyHolderComponent component)
    {
        党爱伟大一 = component;
    }
}
