using IBMU2.UODOTNET;
using M2kClient.M2kADIArray;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace M2kClient
{
    public class M2kCommand
    { 

        /// <summary>
        /// Default Constructor
        /// </summary>
        public M2kCommand()
        { }

        /// <summary>
        /// Will edit any attribute in a Manage 2000 record
        /// ***WARNING***
        /// DO NOT attempt to edit any record value that has business logic associated with it
        /// Contact your ERP administrator if you need help verifing
        /// </summary>
        /// <param name="file">Manage 2000 file to be edited</param>
        /// <param name="recordID">Record ID value to be edited</param>
        /// <param name="attribute">The attribute number that the new value will be associated with, see the warning</param>
        /// <param name="newValue">New value to be written into the record</param>
        /// <param name="arrayCommand">UniDynArray Command to execute on the record</param>
        /// <param name="connection">UniConnection to use for the edit</param>
        /// <returns>Change request error, if none exists then it will return a null value</returns>
        public static string EditRecord(string file, string recordID, int attribute, string newValue, UdArrayCommand arrayCommand, M2kConnection connection)
        {
            try
            {
                using (UniSession uSession = UniObjects.OpenSession(connection.HostName, connection.UserName, connection.Password, connection.UniAccount, connection.UniService))
                {
                    try
                    {
                        using (UniFile uFile = uSession.CreateUniFile(file))
                        {
                            using (UniDynArray udArray = uFile.Read(recordID))
                            {
                                switch(arrayCommand)
                                {
                                    case UdArrayCommand.Insert:
                                        udArray.Insert(attribute, newValue);
                                        break;
                                    case UdArrayCommand.Replace:
                                        udArray.Replace(attribute, newValue);
                                        break;
                                    case UdArrayCommand.Remove:
                                        udArray.Remove(attribute);
                                        break;
                                }
                                uFile.Write(recordID, udArray);
                            }
                        }
                        UniObjects.CloseSession(uSession);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        if (uSession != null)
                        {
                            UniObjects.CloseSession(uSession);
                        }
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Will edit multiple attributes in a Manage 2000 record
        /// ***WARNING***
        /// DO NOT attempt to edit any record value that has business logic associated with it
        /// Contact your ERP administrator if you need help verifing
        /// </summary>
        /// <param name="file">Manage 2000 file to be edited</param>
        /// <param name="recordID">Record ID value to be edited</param>
        /// <param name="attributes">Array of attribute numbers that the new value will be associated with, see the warning</param>
        /// <param name="newValues">Array of New values to be written into the record</param>
        /// <param name="arrayCommand">UniDynArray Command to execute on the record</param>
        /// <param name="connection">UniConnection to use for the edit</param>
        /// <returns>Change request error, if none exists then it will return a null value</returns>
        public static string EditRecord(string file, string recordID, int[] attributes, string[] newValues, UdArrayCommand arrayCommand, M2kConnection connection)
        {
            if (attributes.Length == newValues.Length || arrayCommand == UdArrayCommand.Remove)
            {
                try
                {
                    using (UniSession uSession = UniObjects.OpenSession(connection.HostName, connection.UserName, connection.Password, connection.UniAccount, connection.UniService))
                    {
                        try
                        {
                            using (UniFile uFile = uSession.CreateUniFile(file))
                            {
                                using (UniDynArray udArray = uFile.Read(recordID))
                                {
                                    foreach (var attr in attributes)
                                    {
                                        var attrIndx = Array.IndexOf(attributes, attr);
                                        switch (arrayCommand)
                                        {
                                            case UdArrayCommand.Insert:
                                                udArray.Insert(attr, newValues[attrIndx]);
                                                break;
                                            case UdArrayCommand.Replace:
                                                udArray.Replace(attr, newValues[attrIndx]);
                                                break;
                                            case UdArrayCommand.Remove:
                                                udArray.Remove(attr);
                                                break;
                                        }
                                    }
                                    if (arrayCommand == UdArrayCommand.Replace)
                                    {
                                        uFile.Write(recordID, udArray);
                                    }
                                }
                            }
                            UniObjects.CloseSession(uSession);
                            return null;
                        }
                        catch (Exception ex)
                        {
                            if (uSession != null)
                            {
                                UniObjects.CloseSession(uSession);
                            }
                            return ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "There is either more attributes to edit than new edit values or more edit values have been included that attributes to edit.";
        }

        ///<summary>
        /// Will edit any Multi-Value attribute in a Manage 2000 record
        /// ***WARNING***
        /// DO NOT attempt to edit any record value that has business logic associated with it
        /// Contact your ERP administrator if you need help verifing
        /// </summary>
        /// <param name="file">Manage 2000 file to be edited</param>
        /// <param name="recordID">Record ID value to be edited</param>
        /// <param name="attribute">The attribute number that the new value will be associated with, see the warning</param>
        /// <param name="newValue">Arry of new values to be written into the record</param>
        /// <param name="connection">UniConnection to use for the edit</param>
        /// <returns>All errors are passed back as a string, otherwise it will return a null value</returns>
        public static string EditMVRecord(string file, string recordID, int attribute, string[] newValue, M2kConnection connection)
        {
            try
            {
                using (UniSession uSession = UniObjects.OpenSession(connection.HostName, connection.UserName, connection.Password, connection.UniAccount, connection.UniService))
                {
                    try
                    {
                        using (UniFile uFile = uSession.CreateUniFile(file))
                        {
                            using (UniDynArray udArray = uFile.Read(recordID))
                            {
                                var _udCount = udArray.Dcount(attribute);
                                var counter = 1;
                                foreach (var s in newValue)
                                {
                                    udArray.Replace(attribute, counter, s);
                                    counter++;
                                }
                                if (_udCount >= counter)
                                {
                                    udArray.Replace(attribute, counter, "");
                                    counter++;
                                }
                                uFile.Write(recordID, udArray);
                            }
                        }
                        UniObjects.CloseSession(uSession);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        UniObjects.CloseSession(uSession);
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Get the next available lot number in M2k
        /// </summary>
        /// <param name="connection">Your Manage 2000 Connection object</param>
        /// <returns>
        /// IReadOnlyDictionary file
        /// Key is the Pass/Fail check
        /// On Pass the value will be the lot number created
        /// On Fail the value will be the error message
        /// </returns>
        public static Dictionary<bool, string> GetLotNumber(M2kConnection connection)
        {
            try
            {
                var _subResult = new Dictionary<bool, string>();
                using (UniSession uSession = UniObjects.OpenSession(connection.HostName, connection.UserName, connection.Password, connection.UniAccount, connection.UniService))
                {
                    try
                    {
                        using (UniSubroutine uSubRout = uSession.CreateUniSubroutine("SUB.GET.NEXTLOT", 1))
                        {
                            uSubRout.SetArg(0, "");
                            uSubRout.Call();
                            _subResult.Add(true, uSubRout.GetArg(0));
                            UniObjects.CloseSession(uSession);
                            return _subResult;
                        }
                    }
                    catch (Exception ex)
                    {
                        UniObjects.CloseSession(uSession);
                        _subResult.Add(false, ex.Message);
                        return _subResult;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the next available lot number in M2k
        /// </summary>
        /// <param name="connection">Your Manage 2000 Connection object</param>
        /// <returns>
        /// IReadOnlyDictionary file
        /// Key is the Pass/Fail check
        /// On Pass the value will be the lot number created
        /// On Fail the value will be the error message
        /// </returns>
        public static Dictionary<bool, string> GetDiamondNumber(M2kConnection connection)
        {
            try
            {
                var _subResult = new Dictionary<bool, string>();
                using (UniSession uSession = UniObjects.OpenSession(connection.HostName, connection.UserName, connection.Password, connection.UniAccount, connection.UniService))
                {
                    try
                    {
                        using (UniSubroutine uSubRout = uSession.CreateUniSubroutine("SUB.ARL.NEXT.LOT.NBR", 1))
                        {
                            uSubRout.SetArg(0, "");
                            uSubRout.Call();
                            //TODO:relace static with dynamic code
                            var _m2kArg = $"{DateTime.Today:yMM}{uSubRout.GetArg(0)}".TrimStart('2');
                            _subResult.Add(true, _m2kArg);
                            UniObjects.CloseSession(uSession);
                            return _subResult;
                        }
                    }
                    catch (Exception ex)
                    {
                        UniObjects.CloseSession(uSession);
                        _subResult.Add(false, ex.Message);
                        return _subResult;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Inventory move in the current ERP
        /// </summary>
        /// <param name="stationId">Submitter name</param>
        /// <param name="partNbr">Part number to move</param>
        /// <param name="lotNbr">Lot number to move, leave blank if one does not exist, unit of measure is required with no lot</param>
        /// <param name="uom">Unit of measure, only required on a non-lot transaction</param>
        /// <param name="from">Location the material will be moving from</param>
        /// <param name="to">Location the material will be moving to</param>
        /// <param name="qty">Amount of material to move</param>
        /// <param name="reference">Move reference as open text</param>
        /// <param name="facCode">Facility code</param>
        /// <param name="connection">Current M2k Connection to be used for processing the transaction</param>
        /// <param name="nonConf">Optional: Non-conformance reason associated with the part being moved</param>
        /// <returns>Error number and description, when returned error number is 0 the suffix will be in the description</returns>
        public static Dictionary<int, string> InventoryMove(string stationId, string partNbr, string lotNbr, string uom, string from, string to, int qty, string reference, string facCode, M2kConnection connection, string nonConf = "")
        {
            var _subResult = new Dictionary<int, string>();
            try
            {
                //Move the product
                var suffix = DateTime.Now.ToString($"HHmmssffff");
                var _move = new Locxfer(stationId, partNbr.ToUpper(), from.ToUpper(), to.ToUpper(), qty, lotNbr, reference, facCode, uom.ToUpper());
                File.WriteAllText($"{connection.BTIFolder}LOCXFER{connection.AdiServer}.DAT{suffix}", _move.ToString());
                //Check to see if you need to clear or add a non-conformance reason

                if (!to.EndsWith("N") && !string.IsNullOrEmpty(nonConf))
                {
                    //Remove note
                    EditRecord("LOT.MASTER", $"{lotNbr}|P|{facCode}", 42, "", UdArrayCommand.Remove, connection);
                }
                else if (to.EndsWith("N"))
                {
                    //Add note
                    EditRecord("LOT.MASTER", $"{lotNbr}|P|{facCode}", 42, nonConf, UdArrayCommand.Replace, connection);
                }
                _subResult.Add(0, string.Empty);
                return _subResult;
            }
            catch (Exception ex)
            {
                _subResult.Add(1, ex.Message);
                return _subResult;
            }
        }

        /// <summary>
        /// Post labor in current ERP system within a standard template
        /// </summary>
        /// <param name="stationId">Station ID</param>
        /// <param name="empID">Employee ID</param>
        /// <param name="shift">Employee Shift, used to calc third shift labor varience</param>
        /// <param name="woAndSeq">Work order and sequence seporated by a '*'</param>
        /// <param name="qtyComp">Quantity completed for this transaction</param>
        /// <param name="machID">Machine ID that will receive the labor posting</param>
        /// <param name="clockTranType">Clock transaction type, when passed will need to be formated as 'I' or 'O', by leaving this parameter blank will cause the method to calculate labor</param>
        /// <param name="facCode">Facility code</param>
        /// <param name="connection">Current M2k Connection to be used for processing the transaction</param>
        /// <param name="time">Optional: Post time, must be in a 24 hour clock format using only hours and minutes</param>
        /// <param name="crew">Optional: Crew size, only needs to be passed when the crewsize listed in the ERP is smaller or larger that the amount of crew members posting labor to the work order</param>
        /// <param name="tranDate">Optional: Date of transaction</param>
        /// <returns>Error number and error description, when returned as 0 and a empty string the transaction posted with no errors</returns>
        public static Dictionary<int, string> PostLabor(string stationId, string empID, int shift, string woAndSeq, int qtyComp, string machID, char clockTranType, string facCode, M2kConnection connection, string time = "", int crew = 0, string tranDate = "")
        {
            var _subResult = new Dictionary<int, string>();
            try
            {
                var suffix = DateTime.Now.ToString($"ssffff");
                if (!woAndSeq.Contains('*'))
                {
                    _subResult.Add(1, "Work order or sequence is not in the correct format to pass into M2k.");
                    return _subResult;
                }
                else
                {
                    var _wSplit = woAndSeq.Split('*');
                    if (char.IsWhiteSpace(clockTranType))
                    {
                        if (string.IsNullOrEmpty(tranDate))
                        {
                            if (shift == 3)
                            {
                                if (DateTime.TryParse(time, out DateTime dt))
                                {
                                    if (dt.TimeOfDay > new TimeSpan(19, 00, 00) && dt.TimeOfDay < new TimeSpan(23, 59, 59))
                                    {
                                        if (DateTime.Now.TimeOfDay > new TimeSpan(19,00,00) && DateTime.Now.TimeOfDay < new TimeSpan(23,59,59))
                                        {
                                            tranDate = DateTime.Now.ToString("MM-dd-yyyy");
                                        }
                                        else
                                        {
                                            tranDate = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
                                        }
                                    }
                                    else
                                    {
                                        tranDate = DateTime.Now.ToString("MM-dd-yyyy");
                                    }
                                }
                            }
                            else
                            {
                                tranDate = DateTime.Now.ToString("MM-dd-yyyy");
                            }
                        }
                        var _inDL = new DirectLabor(stationId, empID, 'I', time, _wSplit[0], _wSplit[1], 0, 0, machID, CompletionFlag.N, facCode, crew, tranDate);

                        //posting the clock out time for DateTime.Now

                        time = DateTime.Now.ToString("HH:mm");
                        var _outDL = crew > 0
                            ? new DirectLabor(stationId, empID, 'O', time, _wSplit[0], _wSplit[1], qtyComp, 0, machID, CompletionFlag.N, facCode, crew)
                            : new DirectLabor(stationId, empID, 'O', time, _wSplit[0], _wSplit[1], qtyComp, 0, machID, CompletionFlag.N, facCode);
                        File.WriteAllText($"{connection.SFDCFolder}LB{connection.AdiServer}.DAT{suffix}{empID}", $"{_inDL.ToString()}\n\n{_outDL.ToString()}");
                    }
                    else
                    {
                        var _tempDL = crew > 0
                            ? new DirectLabor(stationId, empID, clockTranType, time, _wSplit[0], _wSplit[1], qtyComp, 0, machID, CompletionFlag.N, facCode, crew)
                            : new DirectLabor(stationId, empID, clockTranType, time, _wSplit[0], _wSplit[1], qtyComp, 0, machID, CompletionFlag.N, facCode);
                        File.WriteAllText($"{connection.SFDCFolder}LB{connection.AdiServer}.DAT{suffix}{empID}", _tempDL.ToString());
                    }
                    _subResult.Add(0, string.Empty);
                    return _subResult;
                }
            }
            catch (Exception ex)
            {
                _subResult.Add(1, ex.Message);
                return _subResult;
            }
        }

        /// <summary>
        /// Process an inventory adjustment in current ERP system with in the standard ADI template
        /// </summary>
        /// <param name="stationId">Station ID</param>
        /// <param name="reference">Adjustment reference</param>
        /// <param name="partNbr">Part Number</param>
        /// <param name="aCode">Adjustment code</param>
        /// <param name="tranOp">Transaction Operation, see adjust object for more information</param>
        /// <param name="tranQty">Transation quantity</param>
        /// <param name="location">Location</param>
        /// <param name="facCode">Facility Code</param>
        /// <param name="connection">Current M2k Connection to be used for processing the transaction</param>
        /// <param name="lot">Optional: Lot number</param>
        /// <returns>Error number and error description, when returned as 0 and a empty string the transaction posted with no errors</returns>
        public static Dictionary<int, string> InventoryAdjustment(string stationId, string reference, string partNbr, AdjustCode aCode, char tranOp, int tranQty, string location, string facCode, M2kConnection connection, string lot = "")
        {
            var _subResult = new Dictionary<int, string>();
            try
            {
                var suffix = DateTime.Now.ToString("ssffff");
                suffix += string.IsNullOrEmpty(lot) ? $"{partNbr}{aCode}" : lot.Replace("-", "");
                if (tranQty <= 0)
                {
                    _subResult.Add(1, "Transaction Quantity must have a value greater then 0.");
                    return _subResult;
                }
                var _tScrap = new Adjust(
                    stationId,
                    facCode,
                    reference,
                    partNbr,
                    aCode,
                    tranOp,
                    tranQty,
                    location,
                    lot);
                File.WriteAllText($"{connection.BTIFolder}ADJUST{connection.AdiServer}.DAT{suffix}", _tScrap.ToString());
                _subResult.Add(0, string.Empty);
                return _subResult;
            }
            catch (Exception ex)
            {
                _subResult.Add(1, ex.Message);
                return _subResult;
            }
        }

        /// <summary>
        /// Process a cycle count entry in current ERP system with in the standard ADI template
        /// </summary>
        /// <param name="stationId">Station ID</param>
        /// <param name="ccNbr">Cycle Count Number</param>
        /// <param name="partNbr">Part Number</param>
        /// <param name="aCode">Adjustment code</param>
        /// <param name="tranQty">Transation quantity</param>
        /// <param name="location">Location</param>
        /// <param name="facCode">Facility Code</param>
        /// <param name="connection">Current M2k Connection to be used for processing the transaction</param>
        /// <param name="lot">Optional: Lot number</param>
        /// <returns>Error number and error description, when returned as 0 and a empty string the transaction posted with no errors</returns>
        public static Dictionary<int, string> CycleCount(string stationId, string ccNbr, string partNbr, AdjustCode aCode, int tranQty, string location, string facCode, M2kConnection connection, string lot = "")
        {
            var _subResult = new Dictionary<int, string>();
            try
            {
                var suffix = DateTime.Now.ToString("ssffff");
                suffix += string.IsNullOrEmpty(lot) ? $"{partNbr}{aCode}" : lot.Replace("-", "");
                if (tranQty < 0)
                {
                    _subResult.Add(1, "Transaction Quantity must have a value greater then 0.");
                    return _subResult;
                }
                var _count = new Cyclect(
                    stationId,
                    facCode,
                    ccNbr,
                    CompletionFlag.N,
                    partNbr,
                    aCode,
                    tranQty,
                    location,
                    lot);
                File.WriteAllText($"{connection.BTIFolder}CYCLECT{connection.AdiServer}.DAT{suffix}", _count.ToString());
                _subResult.Add(0, string.Empty);
                return _subResult;
            }
            catch (Exception ex)
            {
                _subResult.Add(1, ex.Message);
                return _subResult;
            }
        }

        /// <summary>
        /// Process an issue entry in the current ERP system with standard ADI template
        /// </summary>
        /// <param name="stationId">Station ID</param>
        /// <param name="facCode">Facility Code</param>
        /// <param name="partNbr">Part Number</param>
        /// <param name="woNbr">Work Order Number</param>
        /// <param name="rsn">Reason code</param>
        /// <param name="transList">List of issue transations</param>
        /// <param name="connection">Current M2k Connection to be used for processing the transaction</param>
        /// <param name="op">Opetional: Work order sequence or operation</param>
        /// <returns>Error number and error description, when returned as 0 and a empty string the transaction posted with no errors</returns>
        public static Dictionary<int, string> ItemIssue(string stationId, string facCode, string partNbr, string woNbr, string rsn, List<Transaction> transList, M2kConnection connection, string op = "")
        {
            var _subResult = new Dictionary<int, string>();
            try
            {
                var suffix = DateTime.Now.ToString("ssffff");
                var _tIssue = new Issue(
                    stationId,
                    facCode,
                    partNbr,
                    woNbr,
                    rsn,
                    transList);
                if (!string.IsNullOrEmpty(op))
                {
                    _tIssue.Operation = op;
                }
                File.WriteAllText($"{connection.BTIFolder}ISSUE{connection.AdiServer}.DAT{suffix}", _tIssue.ToString());
                _subResult.Add(0, string.Empty);
                return _subResult;
            }
            catch (Exception ex)
            {
                _subResult.Add(1, ex.Message);
                return _subResult;
            }
        }

        //Not implemented yet
        public static Dictionary<int, string> Shipment(string stationId, string compNbr, string woNbr)
        {
            var suffix = DateTime.Now.ToString("HHmmssfff");
            return null;
        }
    }
}
