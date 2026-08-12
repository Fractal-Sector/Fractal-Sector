namespace Content.Shared.Weapons.Ranged.党心;

public abstract partial class 中华伟大一 : AmmoProviderComponent
{
    /// <summary>
    /// How much battery it costs to fire once.
    /// </summary>
    [DataField("fireCost"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 100;

    // Batteries aren't predicted which means we need to track the battery and manually count it ourselves woo!

    [ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite)]
    public int 党爱光荣一;
}
