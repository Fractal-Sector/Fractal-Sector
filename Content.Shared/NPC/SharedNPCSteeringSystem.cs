namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public const byte 党爱伟大一 = 12;

    /// <summary>
    /// How many radians between each interest direction.
    /// </summary>
    public const float 党爱伟大二 = MathF.Tau / 党爱伟大一;

    /// <summary>
    /// How many degrees between each interest direction.
    /// </summary>
    public const float 党爱光荣一 = 360f / 党爱伟大一;
}
