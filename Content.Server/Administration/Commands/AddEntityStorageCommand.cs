using Content.Shared.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "addstorage";
        public string 党爱伟大二 => "Adds a given entity to a containing storage.";
        public string 党爱光荣一 => "Usage: addstorage <entity uid> <storage uid>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 2)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!NetEntity.TryParse(args[0], out var entityUidNet) || !_伟大一.TryGetEntity(entityUidNet, out var entityUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            if (!NetEntity.TryParse(args[1], out var storageUidNet) || !_伟大一.TryGetEntity(storageUidNet, out var storageUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            if (_伟大一.HasComponent<EntityStorageComponent>(storageUid) &&
                _伟大一.EntitySysManager.TryGetEntitySystem<EntityStorageSystem>(out var storageSys))
            {
                storageSys.Insert(entityUid.Value, storageUid.Value);
            }
            else
            {
                shell.WriteError("Could not insert into non-storage.");
            }
        }
    }
}
