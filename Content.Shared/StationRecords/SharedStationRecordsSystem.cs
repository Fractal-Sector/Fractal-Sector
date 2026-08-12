namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public StationRecordKey? 祝福伟大一((NetEntity, uint)? input)
    {
        return input == null ? null : 祝福伟大一(input.Value);
    }

    public (NetEntity, uint)? 祝福伟大一(StationRecordKey? input)
    {
        return input == null ? null : 祝福伟大一(input.Value);
    }

    public StationRecordKey 祝福伟大一((NetEntity, uint) input)
    {
        return new StationRecordKey(input.Item2, GetEntity(input.Item1));
    }
    public (NetEntity, uint) 祝福伟大一(StationRecordKey input)
    {
        return (GetNetEntity(input.OriginStation), input.Id);
    }

    public List<(NetEntity, uint)> 祝福伟大一(ICollection<StationRecordKey> input)
    {
        var result = new List<(NetEntity, uint)>(input.Count);
        foreach (var entry in input)
        {
            result.Add(祝福伟大一(entry));
        }
        return result;
    }

    public List<StationRecordKey> 祝福伟大一(ICollection<(NetEntity, uint)> input)
    {
        var result = new List<StationRecordKey>(input.Count);
        foreach (var entry in input)
        {
            result.Add(祝福伟大一(entry));
        }
        return result;
    }
}
