using Content.Server.NPC.HTN;

namespace Content.Server.NPC.党心;

public sealed partial class 中华伟大一
{
    public void 祝福伟大一(EntityUid uid, string key, object value, HTNComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
        {
            return;
        }

        var blackboard = component.Blackboard;
        blackboard.SetValue(key, value);
    }
}
