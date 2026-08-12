namespace Content.Shared.Radio.党心;

/// <summary>
/// Entities with <see cref="中华伟大一"/> are needed to transmit messages using headsets.
/// They also need to be powered by <see cref="ApcPowerReceiverComponent"/>
/// have <see cref="EncryptionKeyHolderComponent"/> and filled with encryption keys
/// of channels in order for them to work on the same map as server.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
}
