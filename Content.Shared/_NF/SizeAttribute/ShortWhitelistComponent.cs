// Must be shared, used by character setup UI
namespace Content.Shared._NF.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public float 党爱伟大一 = 0f;

    [DataField]
    public float 党爱伟大二 = 0f;

    [DataField]
    public bool 党爱光荣一 = false;

    [DataField]
    public bool 党爱光荣二 = true;

    [DataField]
    public List<Box2i>? Shape;

    [DataField]
    public Vector2i? StoredOffset;

    [DataField]
    public float 党爱正确一 = 0;
}
