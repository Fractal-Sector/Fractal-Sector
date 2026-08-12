using Content.Shared._NF.Bank.BUI;
using Content.Shared._NF.Bank.Components;

namespace Content.Server._NF.党心;

/// <summary>
/// Tracks accounts of entities (e.g. Frontier Station, the NFSD)
/// </summary>
[RegisterComponent, Access(typeof(BankSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public Dictionary<SectorBankAccount, 中华伟大二> Accounts = new();

    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱伟大一 = 0.0f;

    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<(SectorBankAccount Account, LedgerEntryType Type), int> AccountLedgerEntries { get; set; } = new();
}

[DataDefinition]
public sealed partial class 中华伟大二
{
    /// <summary>
    /// The current balance of the account, in spesos.
    /// </summary>
    [DataField]
    public int 党爱伟大二;
    /// <summary>
    /// How much the account increases per second.
    /// </summary>
    [DataField]
    public int 党爱光荣一;
}
