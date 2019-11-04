using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.ViewModels.Customers
{
    public class ColumnViews
    {
        public ColumnViews() { }

        public ColumnViews(string columns)
        {
            string[] splitCols = columns.Split(',');

            foreach (var column in splitCols)
            {
                if (column == "Name")
                    Name = true;
                else if (column == "ADSLPhone")
                    ADSLPhone = true;
                else if (column == "CenterName")
                    CenterName = true;
                else if (column == "LevelTitle")
                    LevelTitle = true;
                else if (column == "LevelTypeTitle")
                    LevelTypeTitle = true;
                else if (column == "BirthDate")
                    BirthDate = true;
                else if (column == "Balance")
                    Balance = true;
                else if (column == "CanDeliverCost")
                    CanDeliverCost = true;
                else if (column == "AgencyName")
                   AgencyName  = true;
                else if (column == "NetworkName")
                    NetworkName = true;
                else if (column == "SuctionModeName")
                    SuctionModeName = true;
                else if (column == "SuctionModeDetailName")
                    SuctionModeDetailName = true;
                else if (column == "FollowStatusName")
                    FollowStatusName = true;
                else if (column == "BuyPossibilityName")
                    BuyPossibilityName = true;
                else if (column == "DocumentStatusName")
                    DocumentStatusName = true;
                else if (column == "Gender")
                    Gender = true;
                else if (column == "Job")
                    Job = true;
                else if (column == "Phone")
                    Phone = true;
                else if (column == "Mobile1")
                    Mobile1 = true;
                else if (column == "Mobile2")
                    Mobile2 = true;
                else if (column == "SLastName")
                    SLastName = true;
                else if (column == "SFirstName")
                    SFirstName = true;
                else if (column == "LegalType")
                    LegalType = true;
                else if (column == "Email")
                    Email = true;
                else if (column == "Address")
                    Address = true;
                else if (column == "Note")
                    Note = true;
                else if (column == "Discontinued")
                    Discontinued = true;
                else if (column == "LevelNikname")
                    LevelNikname = true;
                else if (column == "SkipCenter")
                    SkipCenter = true;
                else if (column == "CreateEmployeeName")
                    CreateEmployeeName = true;
                else if (column == "LevelEntryDate")
                    LevelEntryDate = true;
            }
        }

        public bool Name { get; set; }
        public bool ADSLPhone { get; set; }
        public bool CenterName { get; set; }
        public bool LevelTitle { get; set; }
        public bool LevelTypeTitle { get; set; }
        public bool BirthDate { get; set; }
        public bool Balance { get; set; }
        public bool CanDeliverCost { get; set; }
        public bool AgencyName { get; set; }
        public bool NetworkName { get; set; }
        public bool SuctionModeName { get; set; }
        public bool SuctionModeDetailName { get; set; }
        public bool FollowStatusName { get; set; }
        public bool BuyPossibilityName { get; set; }
        public bool DocumentStatusName { get; set; }
        public bool Gender { get; set; }
        public bool Job { get; set; }
        public bool Phone { get; set; }
        public bool Mobile1 { get; set; }
        public bool Mobile2 { get; set; }
        public bool SLastName { get; set; }
        public bool SFirstName { get; set; }
        public bool LegalType { get; set; }
        public bool Email { get; set; }
        public bool Address { get; set; }
        public bool Note { get; set; }
        public bool Discontinued { get; set; }
        public bool LevelNikname { get; set; }
        public bool SkipCenter { get; set; }
        public bool CreateEmployeeName { get; set; }
        public bool LevelEntryDate { get; set; }
        
    }
}
