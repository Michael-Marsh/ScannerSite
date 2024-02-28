using IBMU2.UODOTNET;
using System;

namespace ScannerSiteBL
{
    public class UnplannedMove
    {
        // class variables for database connection and transactions
        UniSession us1;
        UniFile u1;
        UniSelectList s1;
        UniDynArray a1;
        //public string error = "";
        //public string tracability;
        int flag = 0;
        int checkpoint = -1;
        char mvchar = Convert.ToChar(253);
        int count = 0;

        public UnplannedMove(BarcodeSystem b)
        {
            this.us1 = b.US1;
        }

        public void updatemanage(string stationId, string partNbr, string fromLoc, string toLoc, string moveQty, string traceFlag, string lotNbr)
        {
            string RoutineName = "SUB.WBAR.UNPLANNED.MOVE";
            int IntTotalAtgs = 7;
            string[] largs = new string[IntTotalAtgs];
            
            largs[0] = stationId; largs[1] = partNbr; largs[2] = fromLoc; 
            largs[3] = toLoc; largs[4] = moveQty; largs[5] = traceFlag; largs[6] = lotNbr;
            
            UniSubroutine sub = us1.CreateUniSubroutine(RoutineName, IntTotalAtgs);
            for (int i = 0; i < IntTotalAtgs; i++)
            {
                sub.SetArg(i, largs[i]);
            }
            sub.Call();
        }      

        // validate partnumber entered by the user by just checking if its records exist in the database
        public string  validate_partnbr(string partnbr)
        {
            string error = "";
            string engStatus = "";
            try
            {
                u1 = us1.CreateUniFile("IPL");
                s1 = us1.CreateUniSelectList(0);
                s1.Select(u1);
                try
                {
                    a1 = u1.Read(partnbr);
                    engStatus = a1.Extract(40).ToString();
                    if (engStatus == "D")
                    { error = "Design only part!"; }
                    else { error = "part found"; }
                }
                catch (Exception ex)
                {
                    error = "Part not found!";
                }
            }
            catch (Exception ex)
            {
                error = "Connection timed out!"; 
            }
            return error;
        }

        //checking whether a part number is lot tracable by reading the 150th single valued attribute in the IM table from the database
        public string lot_traceability(string partnbr)
        {
            string traceability = "";
            try
            {
                u1 = us1.CreateUniFile("IM");
                try
                {
                    a1 = u1.Read(partnbr);
                    traceability = a1.Extract(150).ToString();
                }
                catch (Exception ex)
                {
                    traceability = "not specified";

                }
            }
            catch (Exception ex)
            {
                traceability = "Connection timed out!";
            }
            return traceability;
        }

        // validate lot number for the given partnumber and lotnumber by the user by retrieving a list of all the lot numbers for the particular part number and then comparing the provided lot number with the list of retrieved lot numbers.  
        public string validate_lotnbr(string lotnbr, string partnbr)
        {
            string error = "";
            flag = 0;
            try
            {
                u1 = us1.CreateUniFile("LOT.MASTER");
                s1 = us1.CreateUniSelectList(0);
                try
                {
                    s1.Select(u1);
                    s1.SelectMatchingAK(u1, "B$Part_Nbr", partnbr);
                    a1 = s1.ReadList();
                    for (int i = 1; i <= a1.Dcount(); i++)
                    {

                        if (lotnbr == a1.Extract(i).StringValue)
                        {
                            error = "lot found";
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                        error = "Lot not found!";
                    }

                }
                catch (Exception ex)
                {
                    error = "Lot not found!";
                }
            }             
            catch (Exception ex)
            {
                error = "Connection timed out!"; 
            }
            return error;
        }

        //validate the location where the part has to be moved by just checking whether the record of the provided to_location exists in the database(LOC.MASTER table). 
        public string validate_to_location(string to_loc)
        {
            string error = "";
            try
            {
                u1 = us1.CreateUniFile("LOC.MASTER");
                try
                {
                    a1 = u1.Read("01*" + to_loc);
                    error = "To location found";
                }
                catch (Exception ex)
                {
                    error = "To location not found!";
                }
            }             
            catch (Exception ex)
            {
                error = "Connection timed out!"; 
            }
            return error;
        }


        public string[] get_from_location(string from_loc, string partnbr, string lotnum, string traceFlag)
        {
            string[] results = {"",""}; mvchar = Convert.ToChar(253);
            if (traceFlag == "T")
            {
                try
                {
                    u1 = us1.CreateUniFile("LOT.MASTER");
                    s1 = us1.CreateUniSelectList(3);
                    s1.SelectMatchingAK(u1, "B$Part_Nbr", partnbr);
                    a1 = s1.ReadList();
                    for (int i = 1; i <= a1.Dcount(); i++)
                    {
                        if (lotnum == a1.Extract(i).StringValue)
                        {
                            UniDynArray a2 = u1.Read(lotnum);
                            string loc = a2.Extract(8).ToString();
                            string[] loc_arr = loc.Split(mvchar);
                            results[1] = loc_arr[0];
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    results[0] = "Connection timed out!";
                }
            }
            return results;
        }

        public string validate_from_location(string from_loc, string partnbr, string lotnum, string traceFlag)
        {
            string error = "";
            flag = 0;
            checkpoint = -1;
            mvchar = Convert.ToChar(253);

            if (traceFlag == "T")
            {
                try
                {
                    u1 = us1.CreateUniFile("LOT.MASTER");
                    s1 = us1.CreateUniSelectList(3);
                    s1.SelectMatchingAK(u1, "B$Part_Nbr", partnbr);
                    a1 = s1.ReadList();
                    for (int i = 1; i <= a1.Dcount(); i++)
                    {
                        if (lotnum == a1.Extract(i).StringValue)
                        {
                            UniDynArray a2 = u1.Read(lotnum);
                            string loc = a2.Extract(8).ToString();
                            string[] loc_arr = loc.Split(mvchar);
                            for (int j = 0; j < loc_arr.Length; j++)
                            {
                                //Console.WriteLine(loc_arr[j]);
                                if (from_loc == loc_arr[j])
                                {
                                    flag = 1;
                                    checkpoint = j;
                                    error = "valid from_location";
                                    break;
                                }
                            }
                        }
                    }
                    if (flag == 0){ error = "From location doesn't contain this part number!"; }
                }
                catch (Exception ex)
                {
                    error = "Connection timed out!";
                }
            }
            else
            {
            
                try
                {
                    u1 = us1.CreateUniFile("IPL");
                    a1 = u1.Read(partnbr);
                    string loc = a1.Extract(14).ToString();
                    string[] loc_arr = loc.Split(mvchar);
                    for (int i = 0; i < loc_arr.Length; i++)
                    {
                        if (from_loc == loc_arr[i])
                        {
                            //Console.WriteLine(loc_arr.Length);
                            flag = 1;
                            checkpoint = i;
                            error = "valid from_location";
                        }
                    }
                    if (flag == 0)
                    {error = "From location doesn't contain this part number!";}
                }
                catch (Exception ex)
                {
                    error = "Connection timed out!";
                }
            }
            return error;  
        }

        public string validate_quantity(string quantity, string partnbr, string lotnbr, string traceFlag)
        {
            string error = "";
            if (traceFlag == "T")
            {
                try
                {

                    u1 = us1.CreateUniFile("LOT.MASTER");
                    a1 = u1.Read(lotnbr);
                    string qty = a1.Extract(9).ToString();
                    string[] qty_arr = qty.Split(mvchar);
                    if (Convert.ToInt32(quantity) > Convert.ToInt32(qty_arr[checkpoint]))
                    {
                        error = "quantity not supported";
                    }
                    else
                    {
                        error = "valid quantity";
                    }

                }
                catch (Exception ex)
                {
                    error = "Connection timed out!";
                }
            }
            else
            {
                try
                {
                    u1 = us1.CreateUniFile("IPL");
                    a1 = u1.Read(partnbr);
                    string qty = a1.Extract(41).ToString();
                    string[] qty_arr = qty.Split(mvchar);
                    if (Convert.ToInt32(quantity) > Convert.ToInt32(qty_arr[checkpoint]))
                    {
                        error = "quantity not supported";
                    }
                    else
                    {
                        error = "valid quantity";
                    }
                }
                catch (Exception ex)
                {
                    error = "Connection timed out!";
                }                
            }
            return error;           
        }

        public string validate_employee(string emplId)
        {
            string error = "";
            try
            {
                u1 = us1.CreateUniFile("EMPLOYEE.MASTER");

                try
                {

                    a1 = u1.Read(emplId);
                    string authorize = a1.Extract(79).ToString();

                    if (authorize == "Y")
                        error = "valid user";
                    else
                        error = "invalid user";
                }
                catch (Exception ex)
                {
                    error = "invalid user";
                }
            }
            catch (Exception ex)
            {
                error = "Connection timed out!";
            }           

            return error;
        }   
    }
}