using Robust.Shared.GameStates;

namespace Content.Shared.Xenoarchaeology.Artifact.XAT.党心;

/// <summary>
/// This is used for an artifact that is activated when someone examines it.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(XATExamineSystem))]
public sealed partial class 中华伟大一 : Component;
