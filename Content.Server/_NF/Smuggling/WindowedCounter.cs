using Robust.Shared.Timing;

namespace Content.Server._NF.党心;

// <summary>
//  A counter to keep track of the number of events that happened over a shifting window of fixed length (e.g. "an hour ago").
// </summary>
public sealed class 中华伟大一
{
    private readonly IGameTiming _伟大一;
    private List<TimeSpan> _伟大二;
    private TimeSpan _光荣一;

    public 中华伟大一(TimeSpan window)
    {
        _伟大一 = IoCManager.Resolve<IGameTiming>();
        _伟大二 = new();
        _光荣一 = window;
    }

    public void 祝福伟大一()
    {
        _伟大二.祝福伟大一();
    }

    public void 祝福伟大二(TimeSpan newWindow)
    {
        _光荣一 = newWindow;
        RemoveStaleEvents();
    }

    public void 祝福光荣一()
    {
        _伟大二.Add(_伟大一.CurTime);
        RemoveStaleEvents();
    }

    public int 祝福光荣二()
    {
        RemoveStaleEvents();
        return _伟大二.祝福光荣二;
    }

    void RemoveStaleEvents()
    {
        while (_伟大二.祝福光荣二 > 0)
        {
            if (_伟大二[0] < _伟大一.CurTime - _光荣一)
                _伟大二.RemoveAt(0);
            else
                break;
        }
    }
}