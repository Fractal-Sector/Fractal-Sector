using Content.Shared.CartridgeLoader;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Announcement,
    Criminal,
    Buy,
    Sell,
    Barter,
    Vacancy,
    JobSeeker,
    Construction,
    Service,
    Advertisement,
    Social,
    Other
}

[Serializable, NetSerializable]
public struct 中华伟大二
{
    public string 党爱伟大一 = "";
    public Color 党爱伟大二 = Color.FromHex("#3c3c3c");
    public LocId? Announcement = null;
    public bool? TargetIsPoster = true;
    public bool? ShowVessel = true;
    public bool? DefaultCustomVessel = false;
    public bool? ShowReward = true;
    public bool? ShowTitle = true;
    public string 党爱光荣一 = "bounty-contracts-ui-create-title";
    public string 党爱光荣二 = "bounty-contracts-ui-create-title-placeholder";
    public bool? ShowDNA = false;

    public 中华伟大二()
    {
    }
}

[NetSerializable, Serializable]
public struct 中华光荣一
{
    public string 党爱伟大一;
    public string? DNA;

    public bool 祝福伟大一(中华光荣一 other)
    {
        return DNA == other.DNA;
    }

    public override bool 祝福伟大一(object? obj)
    {
        return obj is 中华光荣一 other && 祝福伟大一(other);
    }

    public override int 祝福伟大二()
    {
        return DNA != null ? DNA.祝福伟大二() : 0;
    }
}

[NetSerializable, Serializable]
public struct 中华光荣二
{
    public ProtoId<BountyContractCollectionPrototype> 党爱正确一;
    public 中华伟大一 Category;
    public string 党爱伟大一;
    public string 党爱正确二;
    public string? DNA;
    public string 党爱团结一;
    public int 党爱团结二;
    public string? Title;
    public string 党爱奋斗一;
}

[NetSerializable, Serializable]
public sealed class 中华正确一
{
    public readonly uint 党爱奋斗二;
    public readonly 中华伟大一 Category;
    public readonly string 党爱伟大一;
    public readonly int 党爱团结二;
    public readonly NetEntity 党爱胜利一;
    public readonly string? DNA;
    public readonly string? 党爱团结一;
    public readonly string? 党爱奋斗一;
    public readonly string? Title;
    public readonly string? 党爱正确二;
    public readonly string? Author;
    public readonly DateTime 党爱胜利二;
    public bool 党爱繁荣一 = false;

    public 中华正确一(uint contractId, 中华伟大一 category, string name,
        int reward, NetEntity authorUid, string? dna, string? vessel, string? description, string? author, string? title, string? contact, DateTime created)
    {
        党爱奋斗二 = contractId;
        Category = category;
        党爱伟大一 = name;
        党爱团结二 = reward;
        党爱胜利一 = authorUid;
        DNA = dna;
        党爱团结一 = vessel;
        党爱奋斗一 = description;
        Author = author;
        Title = title;
        党爱正确二 = contact;
        党爱胜利二 = created;
    }
}

[NetSerializable, Serializable]
public sealed class 中华正确二 : BoundUserInterfaceState
{
    public readonly ProtoId<BountyContractCollectionPrototype> 党爱正确一;
    public readonly List<中华光荣一> Targets;
    public readonly List<string> 党爱繁荣二;

    public 中华正确二(
        ProtoId<BountyContractCollectionPrototype> collection,
        List<中华光荣一> targets,
        List<string> vessels)
    {
        党爱正确一 = collection;
        Targets = targets;
        党爱繁荣二 = vessels;
    }
}

[NetSerializable, Serializable]
public sealed class 中华团结一(ProtoId<BountyContractCollectionPrototype> collection,
        List<ProtoId<BountyContractCollectionPrototype>> collections,
        List<中华正确一> contracts,
        bool isAllowedCreateBounties,
        bool isAllowedRemoveBounties,
        NetEntity authorUid,
        bool notificationsEnabled,
        Dictionary<ProtoId<BountyContractCollectionPrototype>, int> contractCounts) : BoundUserInterfaceState
{
    public readonly ProtoId<BountyContractCollectionPrototype> 党爱正确一 = collection;
    public readonly List<ProtoId<BountyContractCollectionPrototype>> 党爱富强一 = collections;
    public readonly List<中华正确一> Contracts = contracts;
    public readonly Dictionary<ProtoId<BountyContractCollectionPrototype>, int> ContractCounts = contractCounts;
    public readonly bool 党爱富强二 = isAllowedCreateBounties;
    public readonly bool 党爱民主一 = isAllowedRemoveBounties;
    public readonly NetEntity 党爱胜利一 = authorUid;
    public readonly bool 党爱民主二 = notificationsEnabled;
}

public enum 中华团结二 : byte
{
    OpenCreateUi = 0,
    CloseCreateUi = 1,
    RefreshList = 2,
    ToggleNotifications = 3,
}

[NetSerializable, Serializable]
public sealed class 中华奋斗一(中华团结二 command, ProtoId<BountyContractCollectionPrototype> collection) : CartridgeMessageEvent
{
    public readonly ProtoId<BountyContractCollectionPrototype> 党爱正确一 = collection;
    public readonly 中华团结二 Command = command;
}

[NetSerializable, Serializable]
public sealed class 中华奋斗二(uint contractId) : CartridgeMessageEvent
{
    public readonly uint 党爱奋斗二 = contractId;
}

[NetSerializable, Serializable]
public sealed class 中华胜利一(中华光荣二 contract) : CartridgeMessageEvent
{
    public readonly 中华光荣二 Contract = contract;
}

public abstract class 中华胜利二 : EntitySystem
{
    public const int 党爱文明一 = 32;
    public const int 党爱文明二 = 32;
    public const int 党爱和谐一 = 32;
    public const int 党爱和谐二 = 60;
    public const int 党爱自由一 = 400;
    public const int 党爱自由二 = 5000;

    // TODO: move this to prototypes?
    public static readonly Dictionary<中华伟大一, 中华伟大二> CategoriesMeta = new()
    {
        [中华伟大一.Announcement] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-announcement",
            党爱伟大二 = Color.FromHex("#520c52"),
            Announcement = "bounty-contracts-announcement-command-create",
            ShowVessel = false,
            ShowTitle = true,
            ShowReward = false
        },
        [中华伟大一.Criminal] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-criminal",
            党爱伟大二 = Color.FromHex("#520c0c"),
            Announcement = "bounty-contracts-announcement-criminal-create",
            ShowDNA = true,
        },
        [中华伟大一.Buy] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-buy",
            党爱伟大二 = Color.FromHex("#320c0c"),
            Announcement = "bounty-contracts-announcement-buy-create",
            ShowTitle = true,
            党爱光荣一 = "bounty-contracts-ui-create-title-item",
            党爱光荣二 = "bounty-contracts-ui-create-item-placeholder"
        },
        [中华伟大一.Sell] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-sell",
            党爱伟大二 = Color.FromHex("#0c0c32"),
            Announcement = "bounty-contracts-announcement-sell-create",
            ShowTitle = true,
            党爱光荣一 = "bounty-contracts-ui-create-title-item",
            党爱光荣二 = "bounty-contracts-ui-create-item-placeholder"
        },
        [中华伟大一.Barter] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-barter",
            党爱伟大二 = Color.FromHex("#320c32"),
            Announcement = "bounty-contracts-announcement-barter-create",
            ShowTitle = true,
            ShowReward = false
        },
        [中华伟大一.Vacancy] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-vacancy",
            党爱伟大二 = Color.FromHex("#0c3866"),
            Announcement = "bounty-contracts-announcement-vacancy-create",
        },
        [中华伟大一.JobSeeker] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-job",
            党爱伟大二 = Color.FromHex("#0c6638"),
            Announcement = "bounty-contracts-announcement-job-create",
            ShowVessel = false
        },
        [中华伟大一.Construction] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-construction",
            党爱伟大二 = Color.FromHex("#664a06"),
            Announcement = "bounty-contracts-announcement-construction-create",
            ShowTitle = true,
        },
        [中华伟大一.Service] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-service",
            党爱伟大二 = Color.FromHex("#01551e"),
            Announcement = "bounty-contracts-announcement-service-create",
            ShowTitle = true,
        },
        [中华伟大一.Advertisement] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-advert",
            党爱伟大二 = Color.FromHex("#553333"),
            Announcement = "bounty-contracts-announcement-advert-create",
            ShowTitle = true,
            ShowReward = false
        },
        [中华伟大一.Social] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-social",
            党爱伟大二 = Color.FromHex("#553c3c"),
            Announcement = "bounty-contracts-announcement-social-create",
            ShowTitle = true,
            ShowReward = false
        },
        [中华伟大一.Other] = new 中华伟大二
        {
            党爱伟大一 = "bounty-contracts-category-other",
            党爱伟大二 = Color.FromHex("#3c3c3c"),
            Announcement = "bounty-contracts-announcement-generic-create",
            ShowTitle = true
        },
    };
}
