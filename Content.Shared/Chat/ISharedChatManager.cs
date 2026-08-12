namespace Content.Shared.党心;

public interface 中华伟大一
{
    void Initialize();
    void SendAdminAlert(string message);
    void SendAdminAlert(EntityUid player, string message);
}
