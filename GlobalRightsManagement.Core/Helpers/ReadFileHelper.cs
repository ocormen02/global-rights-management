

namespace GlobalRightsManagement.Core.Helpers
{
    public static class ReadFileHelper
    {
        private const char Pipe = '|';
        private const char Comma = ',';

        public static ReadOnlySpan<char> ReadField(ref ReadOnlySpan<char> line)
        {
            var i = line.IndexOf(Pipe);

            if (i < 0) 
            { 
                var last = line.Trim(); 
                line = ReadOnlySpan<char>.Empty; 
                
                return last; 
            }

            var field = line[..i].Trim();
            line = line[(i + 1)..];
            return field;
        }

        // Manually splitting by comma to avoid allocations from string.Split
        public static string[] SplitByComma(ReadOnlySpan<char> span)
        {
            var list = new List<string>(4);
            while (!span.IsEmpty)
            {
                var i = span.IndexOf(Comma);
                ReadOnlySpan<char> part;
                
                if (i < 0) 
                { 
                    part = span.Trim(); 
                    span = ReadOnlySpan<char>.Empty; 
                }
                else 
                { 
                    part = span[..i].Trim(); 
                    span = span[(i + 1)..]; 
                }
                
                if (!part.IsEmpty) 
                {
                    list.Add(part.ToString());
                } 
            }
            return list.ToArray();
        }

    }
}
