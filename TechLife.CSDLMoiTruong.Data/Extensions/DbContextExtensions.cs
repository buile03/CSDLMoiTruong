using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TechLife.CSDLMoiTruong.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task<List<TModel>> RawQuery<TModel>(this DbContext context, string queryString)
        {
            return await RawQuery<TModel>(context, queryString, CommandType.Text);
        }

        public static async Task<List<TModel>> RawQuery<TModel>(this DbContext context, string queryString, CommandType type)
        {
            return await RawQuery<TModel>(context, queryString, type, new Dictionary<string, object>());
        }

        public static async Task<List<TModel>> RawQuery<TModel>(this DbContext context, string queryString, Dictionary<string, object> parameters)
        {
            return await RawQuery<TModel>(context, queryString, CommandType.Text, parameters);
        }

        public static async Task<List<TModel>> RawQuery<TModel>(this DbContext context, string queryString, CommandType type, Dictionary<string, object> parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            var sqlParams = parameters.Select(s => new SqlParameter($"@{s.Key}", s.Value ?? DBNull.Value));

            var connection = context.Database.GetDbConnection();

            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            using var cmd = connection.CreateCommand();

            cmd.CommandText = queryString;
            cmd.CommandType = type;
            cmd.CommandTimeout = 50;

            if (sqlParams.Any())
            {
                foreach (var item in sqlParams)
                {
                    cmd.Parameters.Add(item);
                }
            }
            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                var result = reader.GetListData<TModel>();

                await connection.CloseAsync();

                return result;
            }
            catch
            {
                await connection.CloseAsync();
                throw;
            }

            finally { await connection.DisposeAsync(); }
        }

        private static List<TModel> GetListData<TModel>(this IDataReader dr)
        {
            var result = new List<TModel>();

            string columnName;

            // Get all the properties in <TModel>
            PropertyInfo[] props = typeof(TModel).GetProperties();

            while (dr.Read())
            {
                TModel model = Activator.CreateInstance<TModel>();

                // Loop through columns in data reader
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    // Get field name from data reader
                    columnName = dr.GetName(index);

                    // Get property that matches the field name
                    PropertyInfo property = props.FirstOrDefault(col => col.Name == columnName);

                    if (property != null)
                    {
                        // Get the value from the table
                        var value = dr[columnName];
                        // Assign value to property if not null
                        if (!value.Equals(DBNull.Value))
                        {
                            property.SetValue(model, value);
                        }
                    }
                }
                result.Add(model);
            }
            dr.Close();
            return result;
        }

        public static async Task<T> RawSingleValueQuery<T>(this DbContext context, string queryString)
        {
            return await RawSingleValueQuery<T>(context, queryString, CommandType.Text);
        }

        public static async Task<T> RawSingleValueQuery<T>(this DbContext context, string queryString, CommandType type)
        {
            return await RawSingleValueQuery<T>(context, queryString, type, new Dictionary<string, object>());
        }

        public static async Task<T> RawSingleValueQuery<T>(this DbContext context, string queryString, Dictionary<string, object> parameters)
        {
            return await RawSingleValueQuery<T>(context, queryString, CommandType.Text, parameters);
        }

        public static async Task<T> RawSingleValueQuery<T>(this DbContext context, string queryString, CommandType type, Dictionary<string, object> parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            var sqlParams = parameters.Select(s => new SqlParameter($"@{s.Key}", s.Value ?? DBNull.Value));

            var connection = context.Database.GetDbConnection();

            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            using var cmd = connection.CreateCommand();

            cmd.CommandText = queryString;
            cmd.CommandType = type;
            cmd.CommandTimeout = 50;

            if (sqlParams.Any())
            {
                foreach (var item in sqlParams)
                {
                    cmd.Parameters.Add(item);
                }
            }
            try
            {
                var data = await cmd.ExecuteScalarAsync();

                await connection.CloseAsync();

                return (T)data;
            }
            catch
            {
                await connection.CloseAsync();
                throw;
            }
            finally { await connection.DisposeAsync(); }
        }
    }
}