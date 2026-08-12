using Robust.Shared.GameStates;

namespace Content.Shared.Magic.党心;

/// <summary>
/// The <see cref="SharedMagicSystem"/> checks this if a spell requires wizard clothes
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedMagicSystem))]
public sealed partial class 中华伟大一 : Component;
