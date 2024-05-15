using System;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Xml;

namespace M2kClient
{
    internal static class Config
    {
        /// <summary>
        /// Loads a SQL Connection object from the global config file
        /// </summary>
        /// <returns>Config file existance or creation</returns>
        public static SqlConnection GetSqlConnection()
        {
            try
            {
                var ConfigFilePath = "\\\\WAXFS001\\WAXG-SFW\\ShopFloorWorkbench\\GlobalConfig.xml";
                if (!File.Exists(ConfigFilePath))
                {
                    return null;
                }
                using (var rStream = new FileStream(ConfigFilePath, FileMode.Open, FileAccess.Read))
                {
                    var rSettings = new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true };
                    using (var reader = XmlReader.Create(rStream, rSettings))
                    {
                        while (reader.Read())
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    switch (reader.Name)
                                    {
                                        case "SqlConnection":
                                            var pass = new SecureString();
                                            foreach (var c in reader.GetAttribute("ServicePass"))
                                            {
                                                pass.AppendChar(c);
                                            }
                                            pass.MakeReadOnly();
                                            var sqlCred = new SqlCredential(reader.GetAttribute("ServiceUser"), pass);
                                            var sqlCon = new SqlConnection($"Server={reader.GetAttribute("Name")};DataBase=CONTI_MAIN;Connection Timeout={reader.GetAttribute("TimeOut")};MultipleActiveResultSets=True;Connection Lifetime=3;Max Pool Size=3;Pooling=true;", sqlCred);
                                            sqlCon.StatisticsEnabled = true;
                                            return sqlCon;
                                    }
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
