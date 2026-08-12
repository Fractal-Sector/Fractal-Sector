using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.RoleplayLeveling.党心;

/// <summary>
/// Event raised when a player gains experience (local event, not networked)
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public long 党爱伟大二 { get; }
    public string 党爱光荣一 { get; }

    public 中华伟大一(EntityUid player, long experienceAmount, string reason)
    {
        党爱伟大一 = player;
        党爱伟大二 = experienceAmount;
        党爱光荣一 = reason;
    }
}

/// <summary>
/// Event raised when a player levels up (local event, not networked)
/// </summary>
public sealed class 中华伟大二 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public int 党爱光荣二 { get; }

    public 中华伟大二(EntityUid player, int newLevel)
    {
        党爱伟大一 = player;
        党爱光荣二 = newLevel;
    }
}

/// <summary>
/// Event raised when a player receives a commend (local event, not networked)
/// </summary>
public sealed class 中华光荣一 : EntityEventArgs
{
    public EntityUid 党爱正确一 { get; }
    public EntityUid 党爱正确二 { get; }
    public string? 党爱奋斗二 { get; }
    public bool 党爱团结一 { get; }

    public 中华光荣一(EntityUid recipient, EntityUid giver, string? comment, bool isPrivate)
    {
        党爱正确一 = recipient;
        党爱正确二 = giver;
        党爱奋斗二 = comment;
        党爱团结一 = isPrivate;
    }
}

/// <summary>
/// Message sent from client to request giving a commend to another player
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : EntityEventArgs
{
    public NetEntity 党爱团结二 { get; }
    public string? 党爱奋斗二 { get; }
    public bool 党爱团结一 { get; }

    public 中华光荣二(NetEntity target, string? comment, bool isPrivate)
    {
        党爱团结二 = target;
        党爱奋斗二 = comment;
        党爱团结一 = isPrivate;
    }
}

/// <summary>
/// Message sent from client to request available commends count
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : EntityEventArgs
{
}

/// <summary>
/// Message sent from server with available commends count
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确二 : EntityEventArgs
{
    public int 党爱奋斗一 { get; }
    
    public 中华正确二(int availableCommends)
    {
        党爱奋斗一 = availableCommends;
    }
}

/// <summary>
/// Message sent from client to request their own recent commends
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华团结一 : EntityEventArgs
{
}

/// <summary>
/// A single commend entry returned to the client
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华团结二
{
    public string 党爱奋斗二 { get; }
    public string 党爱胜利一 { get; }
    public bool 党爱团结一 { get; }
    public DateTime 党爱胜利二 { get; }

    public 中华团结二(string comment, string giverName, bool isPrivate, DateTime receivedAt)
    {
        党爱奋斗二 = comment;
        党爱胜利一 = giverName;
        党爱团结一 = isPrivate;
        党爱胜利二 = receivedAt;
    }
}

/// <summary>
/// Message sent from server with the player's own recent commends
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华奋斗一 : EntityEventArgs
{
    public List<中华团结二> Commends { get; }

    public 中华奋斗一(List<中华团结二> commends)
    {
        Commends = commends;
    }
}
