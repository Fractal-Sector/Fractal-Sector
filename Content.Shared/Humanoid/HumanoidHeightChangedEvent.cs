namespace Content.Shared.Humanoid;

/// <summary>
/// Raised on a humanoid entity whenever its Height/Width scale actually changes
/// (character creation, or a live effect like the size gun).
/// </summary>
public sealed class HumanoidHeightChangedEvent : EntityEventArgs
{
}
