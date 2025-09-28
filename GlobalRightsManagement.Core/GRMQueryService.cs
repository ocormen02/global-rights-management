using GlobalRightsManagement.Core.Models;


namespace GlobalRightsManagement.Core
{
    public static class GRMQueryService
    {
        public static IEnumerable<MusicContract> Query(
            string contractsPath,
            string partnersPath,
            string partner,
            DateOnly effectiveDate)
        {
            string? neededUsage = FileReaderProcessor.ReadPartnerRules(partnersPath)
                .FirstOrDefault(r => string.Equals(r.Partner, partner, StringComparison.OrdinalIgnoreCase))
                ?.Usage;

            if (string.IsNullOrWhiteSpace(neededUsage))
                return Enumerable.Empty<MusicContract>();


            var filtered = FileReaderProcessor.ReadMusicContract(contractsPath)
                .Where(c => HasUsage(c, neededUsage!) && IsActiveOn(c, effectiveDate))
                .Select(c => new MusicContract(
                    c.Artist,
                    c.Title,
                    new[] { neededUsage! },
                    c.StartDate,
                    c.EndDate
            ))
                .OrderBy(c => c.Artist, StringComparer.OrdinalIgnoreCase)
                .ThenBy(c => c.Title, StringComparer.OrdinalIgnoreCase); 

            return filtered;
        }


        // Core logic filters
        private static bool HasUsage(MusicContract c, string neededUsage) =>
            Array.Exists(c.Usages, u => string.Equals(u, neededUsage, StringComparison.OrdinalIgnoreCase));

        private static bool IsActiveOn(MusicContract c, DateOnly d) =>
            c.StartDate <= d && (c.EndDate is null || d <= c.EndDate);
    }
}
