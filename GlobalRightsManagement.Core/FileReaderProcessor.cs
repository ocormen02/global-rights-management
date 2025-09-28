using GlobalRightsManagement.Core.Helpers;
using GlobalRightsManagement.Core.Models;
using System.Globalization;


namespace GlobalRightsManagement.Core
{
    public class FileReaderProcessor
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        private const string DateFormat = "MM-dd-yyyy";

        public static IEnumerable<MusicContract> ReadMusicContract(string path, bool hasHeader = true)
        {
            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                if (hasHeader) 
                {
                    hasHeader = false; 
                    continue; 
                }

                var span = line.AsSpan();

                var artist = ReadFileHelper.ReadField(ref span);
                var title = ReadFileHelper.ReadField(ref span);
                var usagesS = ReadFileHelper.ReadField(ref span);
                var startS = ReadFileHelper.ReadField(ref span);
                var endS = span.Trim(); 

                if (!DateOnly.TryParseExact(startS, DateFormat, Culture, DateTimeStyles.None, out var start))
                    continue;

                DateOnly? end = null;
                if (!endS.IsEmpty && DateOnly.TryParseExact(endS, DateFormat, Culture, DateTimeStyles.None, out var endParsed))
                    end = endParsed;

                var usages = ReadFileHelper.SplitByComma(usagesS);

                yield return new MusicContract(
                    artist.ToString(),
                    title.ToString(),
                    usages,
                    start,
                    end
                );
            }
        }

        public static IEnumerable<PartnerRule> ReadPartnerRules(string path, bool hasHeader = true)
        {
            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                if (hasHeader) 
                { 
                    hasHeader = false; 
                    continue; 
                }

                var span = line.AsSpan();
                var partner = ReadFileHelper.ReadField(ref span).ToString();
                var usage = span.Trim().ToString();

                if (partner.Length == 0 || usage.Length == 0) 
                {
                    continue;
                } 
                
                yield return new PartnerRule(partner, usage);
            }
        }
    }
}
