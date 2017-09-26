using System.Collections.Generic;

namespace DtoTools.TypeMappers
{
    static class CsToTsTypeMapper
    {
        static readonly Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "string", "text" },
            { "int", "numeric" },
            { "decimal", "numeric" },
            { "DateTime", "date" }
        };

        public static string Map(string value)
        {
            return Mapping.ContainsKey(value) ? Mapping[value] : value;
        }
    }
}