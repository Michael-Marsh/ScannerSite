using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace M2kClient
{
    public class SQLCommand
    {
        /// <summary>
        /// Get whether a product ID exists as a part number or lot number
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>Read only dictionary that contains a key of success or failure and the value is the message</returns>
        public static IReadOnlyDictionary<bool, string> ProductIdExists(string productId)
        {
            var _result = new Dictionary<bool, string>();
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null)
                {
                    sqlCon.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT([Part_Number]) FROM [dbo].[IM-INIT] WHERE [Part_Number] = @p1", sqlCon))
                        {
                            cmd.Parameters.AddWithValue("p1", productId);
                            if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
                            {
                                _result.Add(true, "PN");
                            }
                        }
                        if (_result.Count == 0)
                        {
                            using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT([Lot_Number]) FROM [dbo].[LOT-INIT] WHERE [Lot_Number] = @p1", sqlCon))
                            {
                                if(!productId.Contains("|P"))
                                {
                                    var _productSplit = productId.Split('|');
                                    productId = $"{_productSplit[0]}|P|{_productSplit[1]}";
                                }
                                cmd.Parameters.AddWithValue("p1", productId);
                                if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
                                {
                                    _result.Add(true, "LN");
                                }
                            }
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand(@"SELECT CASE WHEN ISNULL(Lot_Trace, 'N') = 'N' THEN 0 ELSE 1 END FROM [dbo].[IM-INIT] WHERE [Part_Number] = @p1", sqlCon))
                            {
                                cmd.Parameters.AddWithValue("p1", productId);
                                if (Convert.ToInt32(cmd.ExecuteScalar()) == 1)
                                {
                                    _result.Clear();
                                    _result.Add(false, "Need lot number.");
                                }
                            }
                        }
                        return _result;
                    }
                    catch (Exception ex)
                    {
                        _result.Add(false, ex.Message);
                        return _result;
                    }
                }
                else
                {
                    _result.Add(false, "No Connection.");
                    return _result;
                }
            }
        }

        /// <summary>
        /// Get whether a product ID exists as a part number or lot number
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>Read only dictionary that contains a key of success or failure and the value is the message</returns>
        public static IList<string> GetProductByLot(string productId)
        {
            var _result = new List<string>();
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null)
                {
                    sqlCon.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT
	                                                                lot.[Part_Nbr] as 'PartNbr'
	                                                                ,lotloc.[Locations] as 'Loc'
	                                                                ,lotloc.[Oh_Qtys] as 'OH'
	                                                                ,im.[Um] as 'UOM'
                                                                FROM
	                                                                [LOT-INIT] lot
                                                                RIGHT JOIN
	                                                                [LOT-INIT_Lot_Loc_Qtys] lotloc ON lotloc.[ID1] = lot.[Lot_Number]
                                                                RIGHT JOIN
	                                                                [IM-INIT] im ON im.[Part_Number] = lot.[Part_Nbr]
                                                                WHERE
	                                                                lot.[Lot_Number] = @p1", sqlCon))
                        {
                            cmd.Parameters.AddWithValue("p1", productId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    var _counter = 0;
                                    while (reader.Read())
                                    {
                                        if (_counter == 0)
                                        {
                                            //Part Number
                                            _result.Add(reader.GetValue(0).ToString());
                                            //Location
                                            _result.Add(reader.GetValue(1).ToString());
                                            //On Hand
                                            _result.Add(reader.GetValue(2).ToString());
                                            //Unit of Measure
                                            _result.Add(reader.GetValue(3).ToString());
                                            _counter++;
                                        }
                                        else
                                        {
                                            _result.Clear();
                                            _result.Add("Multiple locations, contact supervisor.");
                                        }
                                    }
                                }
                                else
                                {
                                    _result.Add("Nothing to move.");
                                }
                            }
                        }
                        return _result;
                    }
                    catch (Exception ex)
                    {
                        _result.Add(ex.Message);
                        return _result;
                    }
                }
                else
                {
                    _result.Add("No Connection.");
                    return _result;
                }
            }
        }

        /// <summary>
        /// Get a products unit of measure
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <returns>user name as string</returns>
        public static string GetUom(string productId)
        {
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null && !string.IsNullOrEmpty(productId))
                {
                    sqlCon.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT [Um] FROM [dbo].[IM-INIT] WHERE [Part_Number] = @p1", sqlCon))
                        {
                            cmd.Parameters.AddWithValue("p1", productId);
                            return cmd.ExecuteScalar().ToString();
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get a user name based on the user identity
        /// </summary>
        /// <param name="identity">Product ID</param>
        /// <returns>user name as string</returns>
        public static string GetUserName(string identity)
        {
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null && !string.IsNullOrEmpty(identity))
                {
                    sqlCon.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT CONCAT([Last_Name], ', ', [First_Name]) FROM [dbo].[CONTACT-INIT] WHERE [Web_User_ID] = @p1;", sqlCon))
                        {
                            cmd.Parameters.AddWithValue("p1", identity.ToUpper());
                            return cmd.ExecuteScalar().ToString();
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Validate a location
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <returns>true or false for pass or fail</returns>
        public static bool ValidateLocation(string locationId)
        {
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null && !string.IsNullOrEmpty(locationId))
                {
                    sqlCon.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM [dbo].[LOC_MASTER-INIT] WHERE [Location] = @p1 AND [Facility_Code] = '01'", sqlCon))
                        {
                            cmd.Parameters.AddWithValue("p1", locationId);
                            if (int.TryParse(cmd.ExecuteScalar().ToString(), out int i))
                            {
                                return i > 0;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Validate a location has the quantity inputed
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="locationId">Product ID</param>
        /// <param name="qty">Quantity of transaction</param>
        /// <returns>Dictionary with key as true or false for pass or fail and value as the quantity</returns>
        public static IReadOnlyDictionary<bool, string> ValidatePartQuantity(string productId, string locationId, int qty)
        {
            var _result = new Dictionary<bool, string>();
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null && !string.IsNullOrEmpty(locationId))
                {
                    sqlCon.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT [Oh_Qty_By_Loc] FROM [dbo].[IPL-INIT_Location_Data] WHERE [ID1] = @p1 AND [Location] = @p2", sqlCon))
                        {
                            cmd.Parameters.AddWithValue("p1", productId);
                            cmd.Parameters.AddWithValue("p2", locationId);
                            if (int.TryParse(cmd.ExecuteScalar().ToString(), out int i))
                            {
                                if (i >= qty)
                                {
                                    _result.Add(true, "");
                                }
                                else
                                {
                                    _result.Add(false, $"Only {i} available to move.");
                                }
                            }
                            else
                            {
                                _result.Add(false, "No material in from location.");
                            }
                            return _result;
                        }
                    }
                    catch (Exception)
                    {
                        _result.Add(false, "Unknown error.");
                        return _result;
                    }
                }
                else
                {
                    _result.Add(false, "No connection.");
                    return _result;
                }
            }
        }

        /// <summary>
        /// Get a products lot information
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="productType">Product Type</param>
        /// <returns>populated datatable of lot information</returns>
        public static DataTable GetProductTable(string productId, string productType)
        {
            var _result = new Dictionary<bool, string>();
            var _cmdString = productType == "Lot"
                ? "SELECT [LotID] as 'Lot Number', CONCAT([OnHand], ' ', [Uom]) as 'On Hand', [Location] FROM [dbo].[SFW_Lot] WHERE [Sku] = @p1 AND [Type] = @p2"
                : "SELECT CONCAT([OnHand], ' ', [Uom]) as 'On Hand', [Location] FROM [dbo].[SFW_Lot] WHERE [Sku] = @p1 AND [Type] = @p2";
            using (var sqlCon = Config.GetSqlConnection())
            {
                if (sqlCon != null)
                {
                    sqlCon.Open();
                    using (var returnTable = new DataTable())
                    {
                        try
                        {
                            using (SqlDataAdapter adapter = new SqlDataAdapter(_cmdString, sqlCon))
                            {
                                adapter.SelectCommand.Parameters.AddWithValue("p1", productId);
                                adapter.SelectCommand.Parameters.AddWithValue("p2", productType);
                                adapter.Fill(returnTable);
                                return returnTable;
                            }
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
