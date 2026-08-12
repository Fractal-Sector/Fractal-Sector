using Content.Shared.Database;

namespace Content.Shared.Administration.党心;

public interface 中华伟大一
{
    void Add(LogType type, LogImpact impact, ref LogStringHandler handler);

    void Add(LogType type, ref LogStringHandler handler);
}
