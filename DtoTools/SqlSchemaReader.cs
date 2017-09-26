using System.Collections.Generic;
using System.Data.SqlClient;
using DtoTools.TypeMappers;

namespace DtoTools
{
    class SqlSchemaReader
    {
        public static List<DtoPropertyInfo> GetTableInfo(string tableName, string connectionString)
        {
            const string sql = @"
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, (
	SELECT sep.value
    FROM sys.tables st
    INNER JOIN sys.columns sc on st.object_id = sc.object_id
    LEFT JOIN sys.extended_properties sep on st.object_id = sep.major_id
		AND sc.column_id = sep.minor_id
		AND sep.name = 'MS_Description'
    WHERE st.name = c.TABLE_NAME
		AND sc.name = c.COLUMN_NAME
) AS [Description]
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE TABLE_NAME = @tableName";

            var result = new List<DtoPropertyInfo>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@tableName", tableName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DtoPropertyInfo
                        {
                            Name = reader["COLUMN_NAME"].ToString(),
                            Type = SqlToCsTypeMapper.Map(reader["DATA_TYPE"].ToString()),
                            IsNullable = reader["IS_NULLABLE"].ToString() == "YES",
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }
            return result;
        }
    }
}
