using System.Collections.Generic;
using DtoTools.TypeMappers;

namespace DtoTools
{
    class CodeGenerator
    {
        public static string Dto(List<DtoPropertyInfo> properties)
        {
            var result = "";
            foreach (var property in properties)
            {
                var comment = string.IsNullOrEmpty(property.Description) ? "" : $"/// <summary>\n/// {property.Description}\n/// </summary>";
                var attrib = $"[DTOdescription(\"{property.Description}\")]";
                var prop = $"public {property.Type}{(property.IsNullable&& property.Type!="string" ? "?" : "")} {property.Name} {{ get; set; }}";
                result += $"{comment}\n{attrib}\n{prop}\n\n";
            }
            return result;
        }

        public static string Mapping(List<DtoPropertyInfo> properties)
        {
            var result = "";
            foreach (var property in properties)
            {
                var comment = string.IsNullOrEmpty(property.Description) ? "" : $" // {property.Description}";
                result += $"{property.Name} = f.{property.Name},{comment}\n";
            }
            return result;
        }

        public static string GridColumns(List<DtoPropertyInfo> properties)
        {
            var result = "";
            foreach (var property in properties)
            {
                var type = CsToTsTypeMapper.Map(property.Type);
                result += $"{{ width: 60, field: '{property.Name}', title: '{property.Description}', type: '{type}' }},\n";
            }
            return result;
        }

    }
}
