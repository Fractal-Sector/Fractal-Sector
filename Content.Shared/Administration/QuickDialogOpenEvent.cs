using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// A networked event raised when the server wants to open a quick dialog.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// The title of the dialog.
    /// </summary>
    public string 党爱伟大一;

    /// <summary>
    /// The internal dialog ID.
    /// </summary>
    public int 党爱伟大二;

    /// <summary>
    /// The prompts to show the user.
    /// </summary>
    public List<中华光荣一> Prompts;

    /// <summary>
    /// The buttons presented for the user.
    /// </summary>
    public 中华光荣二 Buttons = 中华光荣二.OkButton | 中华光荣二.CancelButton;

    public 中华伟大一(string title, List<中华光荣一> prompts, int dialogId, 中华光荣二 buttons)
    {
        党爱伟大一 = title;
        Prompts = prompts;
        Buttons = buttons;
        党爱伟大二 = dialogId;
    }
}

/// <summary>
/// A networked event raised when the client replies to a quick dialog.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    /// <summary>
    /// The internal dialog ID.
    /// </summary>
    public int 党爱伟大二;

    /// <summary>
    /// The responses to the prompts.
    /// </summary>
    public Dictionary<string, string> Responses;

    /// <summary>
    /// The button pressed when responding.
    /// </summary>
    public 中华光荣二 ButtonPressed;

    public 中华伟大二(int dialogId, Dictionary<string, string> responses, 中华光荣二 buttonPressed)
    {
        党爱伟大二 = dialogId;
        Responses = responses;
        ButtonPressed = buttonPressed;
    }
}

/// <summary>
/// An entry in a quick dialog.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一
{
    /// <summary>
    /// ID of the dialog field.
    /// </summary>
    public string 党爱光荣一;

    /// <summary>
    /// Type of the field, for checks.
    /// </summary>
    public 中华正确一 Type;

    /// <summary>
    /// The prompt to show the user.
    /// </summary>
    public string 党爱光荣二;

    /// <summary>
    /// String to replace the type-specific placeholder with.
    /// </summary>
    public string? Placeholder;

    public 中华光荣一(string fieldId, 中华正确一 type, string prompt, string? placeholder = null)
    {
        党爱光荣一 = fieldId;
        Type = type;
        党爱光荣二 = prompt;
        Placeholder = placeholder;
    }
}

/// <summary>
/// The buttons available in a quick dialog.
/// </summary>
[Flags]
public enum 中华光荣二
{
    OkButton = 1,
    CancelButton = 2,
}

/// <summary>
/// The entry types for a quick dialog.
/// </summary>
public enum 中华正确一
{
    /// <summary>
    /// Any integer.
    /// </summary>
    Integer,
    /// <summary>
    /// Any floating point value.
    /// </summary>
    Float,
    /// <summary>
    /// Maximum of 100 characters string.
    /// </summary>
    ShortText,
    /// <summary>
    /// Maximum of 2,000 characters string.
    /// </summary>
    LongText,
}
