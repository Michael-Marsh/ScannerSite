using IBMU2.UODOTNET;
using System;
using System.Data;

namespace ScannerSiteBL
{
    public class partInfo
    {       
        private string um;
        private string desc;
        private string location;
        private string onHand;
        private DataTable locations;
        private string errorMessage;

        public partInfo() 
        {
            this.um = "";
            this.desc = "";
            this.location = "";
            this.onHand = "";
            this.locations = new DataTable();
            this.errorMessage = "";
        }

        public partInfo(UniDynArray iplPart, UniDynArray imPart)
        {
            //imRow = aRow;
            um = imPart.Extract(3).StringValue;
            desc = imPart.Extract(2).StringValue;
            location =  iplPart.Extract(14).StringValue;
            onHand = iplPart.Extract(41).StringValue;
            
            string engStatus = iplPart.Extract(40).StringValue;
            if (engStatus == "D")
            {
                this.errorMessage = "Design only part!";
                this.locations = new DataTable();
            }
            else { buildLocationTable(); }
        }

        public partInfo(UniDynArray iplPart, UniDynArray imPart, UniDataSet lots)
        {
            um = imPart.Extract(3).StringValue;
            desc = imPart.Extract(2).StringValue;
            location = iplPart.Extract(14).StringValue;
            onHand = iplPart.Extract(41).StringValue;

            string engStatus = iplPart.Extract(40).StringValue;
            if (engStatus == "D")
            {
                this.errorMessage = "Design only part!";
                this.locations = new DataTable();
            }
            else { buildLocationTableForLots(lots); }
        }

        private void buildLocationTable()
        {
            // Create a new DataTable.
            this.locations = new DataTable("Locations");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType,  

            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "LotNumber";
            column.Caption = "Lot Number";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            locations.Columns.Add(column);

            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Location";
            column.Caption = "Location";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            locations.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "OnHand";
            column.Caption = "On Hand";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the column to the table.
            locations.Columns.Add(column);

            // Create three new DataRow objects and add  
            // them to the DataTable 
            char mvchar = Convert.ToChar(253);
            string[] loc = location.Split(mvchar);
            string[] on_hand = onHand.Split(mvchar);
            for (int i = 0; i < loc.Length; i++)
            {
                int qtyTest = 0;
                if (on_hand[i].Trim() != "")
                { qtyTest = System.Convert.ToInt32(on_hand[i].Trim()); }
                if (qtyTest != 0)
                {
                    row = locations.NewRow();
                    row["Location"] = loc[i].Trim();
                    row["OnHand"] = on_hand[i].Trim();
                    locations.Rows.Add(row);
                }
            }

        }

        private void buildLocationTableForLots(UniDataSet lots)
        {
            // Create a new DataTable.
            this.locations = new DataTable("Locations");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType,  

            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "LotNumber";
            column.Caption = "Lot Number";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            locations.Columns.Add(column);

            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Location";
            column.Caption = "Location";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            locations.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "OnHand";
            column.Caption = "On Hand";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the column to the table.
            locations.Columns.Add(column);

            // Create three new DataRow objects and add  
            // them to the DataTable
            foreach (UniRecord item in lots)
            {
                UniDynArray locs = item.Record.Extract(8);
                UniDynArray qtys = item.Record.Extract(9);
                for (int i = 1; i <= locs.Dcount(1); i++)
                {
                    try
                    {
                        int qtyTest = System.Convert.ToInt32(qtys.Extract(1, i).StringValue);
                        if (qtyTest != 0)
                        {
                            row = locations.NewRow();
                            row["LotNumber"] = item.RecordID;
                            row["Location"] = locs.Extract(1, i).StringValue;
                            row["OnHand"] = qtys.Extract(1, i).StringValue;
                            locations.Rows.Add(row);
                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }

        #region Accessors


        public DataTable Locations
        {
            get { return this.locations; }
        }

        public string Um
        { 
            get { return this.um; } 
        }

        public string Desc
        {
            get { return this.desc; }        
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
        }
        
        #endregion

    }
}

