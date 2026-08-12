namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    /// <summary>
    /// Sets the rotation an entity will have when it is "horizontal"
    /// </summary>
    public void 祝福伟大一(Entity<RotationVisualsComponent?> ent, Angle angle)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        if (ent.Comp.HorizontalRotation.Equals(angle))
            return;

        ent.Comp.HorizontalRotation = angle;
        Dirty(ent);
    }


    /// <summary>
    /// Resets the rotation an entity will have when it is "horizontal" back to it's default value.
    /// </summary>
    public void 祝福伟大二(Entity<RotationVisualsComponent?> ent)
    {
        if (Resolve(ent, ref ent.Comp, false))
            祝福伟大一(ent, ent.Comp.DefaultRotation);
    }
}
