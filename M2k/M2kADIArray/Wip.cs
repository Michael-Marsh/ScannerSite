using System;
using System.Collections.Generic;
using System.Linq;

namespace M2kClient.M2kADIArray
{
    public class Wip
    {
        #region Properties

        /// <summary>
        /// Field 1
        /// Transaction Type
        /// Statically set to WIP
        /// </summary>
        public string TranType { get { return "WIP"; } }

        /// <summary>
        /// Field 2
        /// Transaction Station ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// Field 3
        /// Transaction Time
        /// Statically set to the time of the transaction on a 24 hour clock
        /// </summary>
        public string TranTime { get { return DateTime.Now.ToString("HH:mm"); } }

        /// <summary>
        /// Field 4
        /// Transaction Date
        /// Statically set to date of transaction using MM-dd-yyyy as model
        /// </summary>
        public string TranDate { get { return DateTime.Today.ToString("MM-dd-yyyy"); } }

        /// <summary>
        /// Field 5
        /// Facility Code
        /// </summary>
        public string FacilityCode { get; set; }

        /// <summary>
        /// Field 6
        /// Parent Work Order Number
        /// </summary>
        public string WorkOrderNbr { get; set; }

        /// <summary>
        /// Field 7
        /// Parent Quantity Received
        /// </summary>
        public int QtyReceived { get; set; }

        /// <summary>
        /// Field 8
        /// Completion Flag
        /// </summary>
        public CompletionFlag CFlag { get; set; }

        /// <summary>
        /// Field 9
        /// Work order operation or sequence
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Fields 10,11,12
        /// List of each display information object
        /// </summary>
        public List<DisplayInfo> DisplayInfoList { get; set; }

        /// <summary>
        /// Field 14
        /// Parent Receipt Location
        /// To be treated as a 'To' location not a 'From' location
        /// </summary>
        public string RcptLocation { get; set; }

        /// <summary>
        /// Field 15
        /// Parent lot number
        /// When none exists one will be assigned to the transaction
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// Fields 24,25,26,27,70,71
        /// List of all child component objects and their information
        /// </summary>
        public List<CompInfo> ComponentInfoList { get; set; }

        /// <summary>
        /// List of Adjust objects for scrapping out any components during the wip process
        /// </summary>
        public List<Adjust> AdjustmentList { get; set; }

        #endregion

        /// <summary>
        /// Defualt constructor
        /// </summary>
        public Wip()
        { }

        /// <summary>
        /// Method Override
        /// Takes the object and deliminates it along with adding in the referenced field tag numbers
        /// </summary>
        /// <returns>Standard Wip ADI string needed for the BTI to read</returns>
        public override string ToString()
        {
            //First Line of the Transaction:
            //1~Transaction type~2~Station ID~3~Transaction time~4~Transaction date~5~FacilityCode~6~Work order number~7~Quantity received~8~Complete Flag~9~Operation~14~Receipt Location
            //Second and Subsequent Lines of the Transaction:
            //10~Disp code~11~Disp reference~12~Disp quantity
            //15~Lot number
            //24~Component #1 lot number~26~Component #1 work order~25~Component #1 item number~27~Component # 1 lot quantity~70~Component # 1 Issue location~71~static Y
            //24~Component #2 lot number~26~Component #2 work order~25~Component #2 item number~27~Component # 2 lot quantity~70~Component # 2 Issue location~71~static Y
            //99~COMPLETE
            //Must meet this format in order to work with M2k

            var _rValue = $"1~{TranType}~2~{StationId}~3~{TranTime}~4~{TranDate}~5~{FacilityCode}~6~{WorkOrderNbr}~7~{QtyReceived}~8~{CFlag}~9~{Operation}~14~{RcptLocation}";
            foreach (var disp in DisplayInfoList)
            {
                _rValue += $"\n10~{disp.Code}~11~{disp.Reference}~12~{disp.Quantity}";
            }
            if (!string.IsNullOrEmpty(Lot))
            {
                _rValue += $"\n15~{Lot.Trim()}|P|{FacilityCode}";
            }
            foreach (var c in ComponentInfoList.Where(o => !string.IsNullOrEmpty(o.Lot)))
            {
                _rValue += $"\n24~{c.Lot}|P|{FacilityCode}~26~{WorkOrderNbr}~25~{c.PartNbr}|{FacilityCode}~27~{c.Quantity}";
            }
            _rValue += $"\n75~{RcptLocation}~99~COMPLETE";

            return _rValue;
        }
    }
}