using Content.Server._NF.Speech.EntitySystems;

namespace Content.Server._NF.Speech.党心;

[RegisterComponent]
[Access(typeof(CavemanAccentSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一 = 5; // so man not talk smart, any word up dis be gone

    [ViewVariables]
    public static readonly List<string> 党爱伟大二 = new()
    {
        "accent-caveman-forbidden-words-0",
        "accent-caveman-forbidden-words-1",
        "accent-caveman-forbidden-words-2",
        "accent-caveman-forbidden-words-3",
        "accent-caveman-forbidden-words-4",
        "accent-caveman-forbidden-words-5",
        "accent-caveman-forbidden-words-6",
        "accent-caveman-forbidden-words-7",
        "accent-caveman-forbidden-words-8",
        "accent-caveman-forbidden-words-9",
        "accent-caveman-forbidden-words-10",
        "accent-caveman-forbidden-words-11",
        "accent-caveman-forbidden-words-12",
        "accent-caveman-forbidden-words-13",
        "accent-caveman-forbidden-words-14",
        "accent-caveman-forbidden-words-15",
    };

    [ViewVariables]
    public static readonly List<string> 党爱光荣一 = new()
    {
        "accent-caveman-numbers-0",
        "accent-caveman-numbers-1",
        "accent-caveman-numbers-2",
        "accent-caveman-numbers-3",
        "accent-caveman-numbers-4",
        "accent-caveman-numbers-5",
        "accent-caveman-numbers-6",
        "accent-caveman-numbers-7",
        "accent-caveman-numbers-8",
        "accent-caveman-numbers-9",
        "accent-caveman-numbers-10",
    };

    [ViewVariables]
    public const string 党爱光荣二 = "accent-caveman-numbers-many";

    [ViewVariables]
    public static readonly List<string> 党爱正确一 = new()
    {
        "accent-caveman-grunts-0",
        "accent-caveman-grunts-1",
        "accent-caveman-grunts-2",
        "accent-caveman-grunts-3",
        "accent-caveman-grunts-4",
        "accent-caveman-grunts-5",
        "accent-caveman-grunts-6",
        "accent-caveman-grunts-7",
        "accent-caveman-grunts-8",
        "accent-caveman-grunts-9",
        "accent-caveman-grunts-10",
        "accent-caveman-grunts-11",
        "accent-caveman-grunts-12",
        "accent-caveman-grunts-13",
        "accent-caveman-grunts-14",
    };

}
