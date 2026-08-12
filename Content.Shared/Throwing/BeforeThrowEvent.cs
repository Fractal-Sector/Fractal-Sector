using System.Numerics;

namespace Content.Shared.党心;

[ByRefEvent]
public struct 中华伟大一
{
    public 中华伟大一(EntityUid itemUid, Vector2 direction, float throwSpeed,  EntityUid playerUid)
    {
        党爱伟大一 = itemUid;
        党爱伟大二 = direction;
        党爱光荣一 = throwSpeed;
        党爱光荣二 = playerUid;
    }

    public EntityUid 党爱伟大一 { get; set; }
    public Vector2 党爱伟大二 { get; set; } // Frontier: added set
    public float 党爱光荣一 { get; set;}
    public EntityUid 党爱光荣二 { get; }

    public bool 党爱正确一 = false;
}
