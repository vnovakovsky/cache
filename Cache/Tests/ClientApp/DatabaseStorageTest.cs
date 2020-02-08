using System;
using System.Collections.Generic;
using System.Linq;
using Cache;
using ClientApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ClientApp
{
    [TestClass]
    public class DatabaseStorageTest
    {
        [TestMethod]
        // read -> compare -> change -> write -> read -> compare -> rollback -> write
        public void ReadLine_WriteTest()
        {
            DatabaseStorage<int, string> storage = new DatabaseStorage<int, string>();
            List<Word> words = storage.ReadLine(42, 5);
            Word word42 = words[0];
            Employee employee = (Employee)Util.ByteArrayToObject(word42.Buffer);

            Assert.AreEqual(int.Parse(employee.UserID), 42);
            Assert.IsTrue(employee.NationalIDNumber.SequenceEqual("339712426      "));
            Assert.IsTrue(employee.LoginID.SequenceEqual(@"adventure-works\james0"));
            Assert.AreEqual(employee.OrganizationLevel,4);
            Assert.IsTrue(employee.JobTitle.SequenceEqual("Production Technician - WC60                      "));
            Assert.AreEqual(employee.BirthDate, DateTime.Parse("1978-08-26"));
            Assert.IsTrue(employee.MaritalStatus.SequenceEqual("M"));
            Assert.IsTrue(employee.Gender.SequenceEqual("M"));
            Assert.AreEqual(employee.HireDate, DateTime.Parse("2003-01-28"));
            Assert.AreEqual(employee.VacationHours, 39);
            Assert.AreEqual(employee.SickLeaveHours, 39);

            // temporarily change values
            employee.SickLeaveHours = 50;
            employee.NationalIDNumber = "123456789      ";

            byte[] buf = Util.ObjectToByteArray(employee);

            storage.WriteWord(42, buf);

            List<Word> wordsWritten = storage.ReadLine(42, 5);
            Word word42Written = wordsWritten[0];

            Employee employeeWritten = (Employee)Util.ByteArrayToObject(word42Written.Buffer);

            Assert.AreEqual(int.Parse(employeeWritten.UserID), 42);
            Assert.IsTrue(employeeWritten.NationalIDNumber.SequenceEqual("123456789      "));
            Assert.IsTrue(employeeWritten.LoginID.SequenceEqual(@"adventure-works\james0"));
            Assert.AreEqual(employeeWritten.OrganizationLevel, 4);
            Assert.IsTrue(employeeWritten.JobTitle.SequenceEqual("Production Technician - WC60                      "));
            Assert.AreEqual(employeeWritten.BirthDate, DateTime.Parse("1978-08-26"));
            Assert.IsTrue(employeeWritten.MaritalStatus.SequenceEqual("M"));
            Assert.IsTrue(employeeWritten.Gender.SequenceEqual("M"));
            Assert.AreEqual(employeeWritten.HireDate, DateTime.Parse("2003-01-28"));
            Assert.AreEqual(employeeWritten.VacationHours, 39);
            Assert.AreEqual(employeeWritten.SickLeaveHours, 50);

            // rollback original values
            employeeWritten.SickLeaveHours = 39;
            employeeWritten.NationalIDNumber = "339712426      ";
            buf = Util.ObjectToByteArray(employeeWritten);

            storage.WriteWord(42, buf);
        }
    }
}
