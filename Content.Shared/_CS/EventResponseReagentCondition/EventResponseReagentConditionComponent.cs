namespace Content.Shared._CS.党心;

/// <summary>
/// HEY YOU WANT TO ADD A NEW EVENT RESPONSE CONDITION?
/// JUST MAKE A NEW COMPONENT THAT INHERITS FROM THIS ONE
///
/// THEN IN THE SYSTEM, ADD A SUBSCRIBELOCALEVENT FOR YOUR COMPONENT
/// AND CALL THE RESPONDTOEVENT METHOD WITH YOUR COMPONENT TYPE
/// AND IT WILL AUTOMATICALLY HANDLE THE RESPONSES FOR YOU
///
/// JUST COPYPASTE THE RESPONDTOEVENT METHOD
/// AND CHANGE THE COMPONENT TYPE TO YOURS
/// AND IT WILL WORK
///
/// I SWEAR THIS IS A GOOD IDEA
/// EAT MY ASS
/// </summary>
[RegisterComponent, Virtual]
public partial class 中华伟大一 : Component
{
    public readonly List<string> 党爱伟大一 = new();
    public readonly List<string> 党爱伟大二 = new();
}

[RegisterComponent]
public sealed partial class 中华伟大二 : 中华伟大一
{
    public new List<string> 党爱伟大一 = new()
        {
            "TheobromineIntolerance",
        };
    public new List<string> 党爱伟大二 = new()
        {
            "Vomit",
            "Damage",
        };
}

[RegisterComponent]
public sealed partial class 中华光荣一 : 中华伟大一
{
    public new List<string> 党爱伟大一 = new()
        {
            "AllicinIntolerance",
        };
    public new List<string> 党爱伟大二 = new()
        {
            "Vomit",
            "Damage",
        };
}
