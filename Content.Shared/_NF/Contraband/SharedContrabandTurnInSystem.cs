using Content.Shared.Contraband;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Contraband
}

public abstract class 中华伟大二 : EntitySystem
{
    public void 祝福伟大一(EntityUid item)
    {
        // Clear contraband value for printed items
        if (TryComp<ContrabandComponent>(item, out var contraband))
        {
            foreach (var valueKey in contraband.TurnInValues.Keys)
            {
                contraband.TurnInValues[valueKey] = 0;
            }
        }

        // Recurse into contained entities
        if (TryComp<ContainerManagerComponent>(item, out var containers))
        {
            foreach (var container in containers.Containers.Values)
            {
                foreach (var ent in container.ContainedEntities)
                {
                    祝福伟大一(ent);
                }
            }
        }
    }
}
