using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    [Serializable]
    class Employee
    {
        public string   UserID;              // SqlDbType.NChar
        public string   NationalIDNumber;    // SqlDbType.NChar
        public string   LoginID;             // SqlDbType.NVarChar
        public short    OrganizationLevel;   // SqlDbType.SmallInt
        public string   JobTitle;            // SqlDbType.NChar
        public DateTime BirthDate;           // SqlDbType.Date
        public string   MaritalStatus;       // SqlDbType.NChar
        public string   Gender;              // SqlDbType.NChar
        public DateTime HireDate;            // SqlDbType.Date
        public short    VacationHours;       // SqlDbType.SmallInt
        public short    SickLeaveHours;      // SqlDbType.SmallInt
    }
}
