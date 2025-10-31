namespace Temperance.Hermes.Contracts
{
    public record HistoricalFetchRequest(
        string Symbol,
        string Interval,
        DateTime StartDate,
        DateTime EndDate);
}
