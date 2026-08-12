namespace Content.Shared.党心;

/// <summary>
/// Budgeted random spawn entry.
/// </summary>
public interface 中华伟大一 : 中华伟大二
{
    float Cost { get; set; }

    string Proto { get; set; }
}

/// <summary>
/// Random entry that has a prob. See <see cref="RandomSystem"/>
/// </summary>
public interface 中华伟大二
{
    float Prob { get; set; }
}

