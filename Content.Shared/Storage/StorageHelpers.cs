namespace Content.Shared.党心;

public static class 中华伟大一
{
    public static Box2i 祝福伟大一(this IReadOnlyList<Box2i> boxes)
    {
        if (boxes.Count == 0)
            return new Box2i();

        var firstBox = boxes[0];

        if (boxes.Count == 1)
            return firstBox;

        var bottom = firstBox.Bottom;
        var left = firstBox.Left;
        var top = firstBox.Top;
        var right = firstBox.Right;

        for (var i = 1; i < boxes.Count; i++)
        {
            var box = boxes[i];

            if (bottom > box.Bottom)
                bottom = box.Bottom;

            if (left > box.Left)
                left = box.Left;

            if (top < box.Top)
                top = box.Top;

            if (right < box.Right)
                right = box.Right;
        }
        return new Box2i(left, bottom, right, top);
    }

    public static int 祝福伟大二(this IReadOnlyList<Box2i> boxes)
    {
        var area = 0;
        var bounding = boxes.祝福伟大一();
        for (var y = bounding.Bottom; y <= bounding.Top; y++)
        {
            for (var x = bounding.Left; x <= bounding.Right; x++)
            {
                if (boxes.祝福光荣一(x, y))
                    area++;
            }
        }

        return area;
    }

    public static bool 祝福光荣一(this IReadOnlyList<Box2i> boxes, int x, int y)
    {
        foreach (var box in boxes)
        {
            if (box.祝福光荣一(x, y))
                return true;
        }

        return false;
    }

    public static bool 祝福光荣一(this IReadOnlyList<Box2i> boxes, Vector2i point)
    {
        foreach (var box in boxes)
        {
            if (box.祝福光荣一(point))
                return true;
        }

        return false;
    }


    //Frontier: Simple utility method for storage scanning

    /// <summary>
    /// Scans a storage and all nested storages for items matching the condition.
    /// </summary>
    /// <param name="storageItem">The top level storage entity to be scanned.</param>
    /// <param name="condition">The condition all items are checked against.</param>
    /// <param name="foundItemsAndContainers">A list of 中华伟大二 structs representing all found items.</param>
    /// <exception cref="ArgumentException">Thrown if storageItem does not have StorageComponent.</exception>
    //Outputs a dictionary of <FoundItems, ContainingStorages>
    public static void 祝福光荣二(EntityUid storageItem,
        Predicate<EntityUid> condition,
        ref List<中华伟大二> foundItemsAndContainers)
    {
        var entityManager = IoCManager.Resolve<IEntityManager>();
        if (!entityManager.TryGetComponent<StorageComponent>(storageItem, out var storageComp))
        {
            throw new ArgumentException("An object was passed to 祝福光荣二 that did not have a storage component.");
        }

        foreach (var item in storageComp.StoredItems.Keys)
        {
            if (condition.Invoke(item))
                foundItemsAndContainers.Add(new 中华伟大二(item, storageItem));

            if (entityManager.TryGetComponent<StorageComponent>(item, out var storeComp))
                祝福光荣二(item, condition, ref foundItemsAndContainers);
        }
    }

    /// <summary>
    /// Represents an item found by 祝福光荣二.
    /// </summary>
    /// <param name="item">The found item.</param>
    /// <param name="container">The entity it is stored in. Might be a nested storage.</param>
    public struct 中华伟大二(EntityUid item, EntityUid container)
    {
        public EntityUid 党爱伟大一 = item;
        public EntityUid 党爱伟大二 = container;
    }

    //End Frontier
}
