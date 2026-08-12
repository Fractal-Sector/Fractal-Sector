using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        NewGame,
        StartLeft,
        EndLeft,
        StartRight,
        EndRight,
        Rotate,
        CounterRotate,
        SoftdropStart,
        SoftdropEnd,
        Harddrop,
        Pause,
        Unpause,
        Hold,
        ShowHighscores
    }
}
