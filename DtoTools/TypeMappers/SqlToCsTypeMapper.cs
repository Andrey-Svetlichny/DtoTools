using System.Collections.Generic;

namespace DtoTools.TypeMappers
{
    static class SqlToCsTypeMapper
    {
        static readonly Dictionary<string, string> Mapping = new Dictionary<string, string>
            {
                {"tinyint", "int"},
                {"smallint", "int"},
                {"bigint", "int"},
                {"numeric", "decimal"},
                {"datetime", "DateTime"},
                {"varchar", "string"}
            };

        public static string Map(string value)
        {
            return Mapping.ContainsKey(value) ? Mapping[value] : value;
        }
    }
}