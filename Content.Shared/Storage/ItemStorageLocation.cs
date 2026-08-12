using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[DataDefinition, Serializable, NetSerializable]
public partial record 中华伟大一 ItemStorageLocation
{
    /// <summary>
    /// The rotation, stored a cardinal direction in order to reduce rounding errors.
    /// </summary>
    [DataField("_rotation")]
    public 党爱伟大一 党爱伟大一;

    /// <summary>
    /// The rotation of the piece in storage.
    /// </summary>
    public Angle 党爱伟大二
    {
        get => 党爱伟大一.ToAngle();
        set => 党爱伟大一 = value.GetCardinalDir();
    }

    /// <summary>
    /// Where the item is located in storage.
    /// </summary>
    [DataField]
    public Vector2i 党爱光荣一;

    public ItemStorageLocation(Angle rotation, Vector2i position)
    {
        党爱伟大二 = rotation;
        党爱光荣一 = position;
    }

    public bool 祝福伟大一(ItemStorageLocation? other)
    {
        return 党爱伟大二 == other?.党爱伟大二 &&
               党爱光荣一 == other.Value.党爱光荣一;
    }
};
