using System;
using System.ComponentModel;

namespace M2kClient
{
    public static class Extensions
    {
        /// <summary>
        /// Change the database that the M2kConnection is currently using
        /// </summary>
        /// <param name="connection">Current M2kConnection Object</param>
        /// <param name="database">New database to use</param>
        /// <param name="facCode">M2k Facility to use</param>
        public static void DatabaseChange(this M2kConnection connection, Database database, int facCode)
        {
            connection.Facility = facCode;
            connection.Database = database;
            connection.BTIFolder = $"\\\\WAXAS001\\WAXAS001-BTI.TRANSACTIONS\\";
            connection.SFDCFolder = $"\\\\WAXAS001\\WAXAS001-SFDC.TRANSACTIONS\\";
        }

        /// <summary>
        /// Get an enum value from its description attribute
        /// </summary>
        /// <typeparam name="T">Enum value</typeparam>
        /// <param name="description">Description attribute to search for</param>
        /// <returns>Enum value</returns>
        public static string GetValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return ((T)field.GetValue(null)).ToString();
                }
                else
                {
                    if (field.Name == description)
                        return ((T)field.GetValue(null)).ToString();
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
