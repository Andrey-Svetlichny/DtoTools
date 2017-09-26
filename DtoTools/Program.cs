using System;
using System.IO;
using System.Linq;

namespace DtoTools
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "-t")
            {
                GenerateFromDbTable(args[1]);
                return;
            }
            if (args.Length == 2 && args[0] == "-f")
            {
                GenerateFromCsFile(args[1]);
                return;
            }
            Console.WriteLine("DTO tools V-1.0");
            Console.WriteLine("(c) Andrey Svetlichny 2017\n");
            Console.WriteLine("DtoTools -t <TableName>\n\tGenerate DTO, mapping and other files from MS SQL Schema for TableName (connectionStrings is configured in .config)");
            Console.WriteLine("DtoTools -f <FileName>\n\tGenerate mapping and other files from C# DTO .cs file");
        }

        /// <summary>
        /// Read .cs DTO source file and create mapping.
        /// </summary>
        private static void GenerateFromCsFile(string filePath)
        {
            Console.WriteLine($"Reading and parsing .cs file \"{filePath}\" ...");
            var dtoSourceCode = File.ReadAllText(filePath);
            var propertyInfos = DtoParser.Parse(dtoSourceCode).ToList();
            if (propertyInfos.Count == 0)
            {
                Console.WriteLine("No properties found");
            }
            else
            {
                Console.WriteLine("Generating files...");
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                File.WriteAllText(fileName + "GridColumns.ts", CodeGenerator.GridColumns(propertyInfos));
                File.WriteAllText(fileName + "Mapping.cs", CodeGenerator.Mapping(propertyInfos));
                Console.WriteLine("Done.");
            }
        }

        /// <summary>
        /// Read MS SQL Schema and create DTO content.
        /// </summary>
        private static void GenerateFromDbTable(string tableName)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            Console.WriteLine($"Reading Schema for {tableName}... ");
            var data = SqlSchemaReader.GetTableInfo(tableName, connectionString);
            if (data.Count == 0)
            {
                Console.WriteLine("No data found");
            }
            else
            {
                Console.WriteLine("Generating DTO...");
                File.WriteAllText(tableName + "DTO.cs", CodeGenerator.Dto(data));
                File.WriteAllText(tableName + "Mapping.cs", CodeGenerator.Mapping(data));
                Console.WriteLine("Done.");
            }
        }
    }
}
