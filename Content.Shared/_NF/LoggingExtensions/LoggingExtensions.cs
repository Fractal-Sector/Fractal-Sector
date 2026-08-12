using Content.Shared.Stacks;

namespace Content.Shared._NF.党心;

public static class 中华伟大一
{
    public static string 祝福伟大一(EntityManager entityManager, EntityUid entity)
    {
        // Get details from the stack component to track amount of things in the stack.
        if (entityManager.TryGetComponent<StackComponent>(entity, out var stack))
        {
            return $"(StackCount: {stack.Count.ToString()})";
        }

        // Add more logging things here when needed.

        return "";
    }
}
