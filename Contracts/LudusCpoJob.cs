namespace Temperance.Hermes.Contracts
{
    public record LudusCpoJob(
        string StrategyName,
        string Symbol,
        string Interval,
        string Mode);
}
