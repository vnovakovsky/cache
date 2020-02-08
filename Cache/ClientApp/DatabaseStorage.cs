using Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace ClientApp
{
    class DatabaseStorage<Key, Value> : Cache.IStorage<Key>
    {
        SqlConnection conn_;
        internal DatabaseStorage()
        {
            conn_ = new SqlConnection(
              "server=WOOD;uid=CacheUser;pwd=CacheUser;database=AdventureWorks");
            conn_.Open();
        }
        // not used
        public byte[] ReadWord(Key key)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(Key key, byte[] binWord)
        {
            SqlCommand cmd = new SqlCommand(
              "update_new_employee", conn_);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.NChar);
                cmd.Parameters.Add("@NationalIDNumber", SqlDbType.NChar);
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@OrganizationLevel", SqlDbType.SmallInt);
                cmd.Parameters.Add("@JobTitle", SqlDbType.NChar);
                cmd.Parameters.Add("@BirthDate", SqlDbType.Date);
                cmd.Parameters.Add("@MaritalStatus", SqlDbType.NChar);
                cmd.Parameters.Add("@Gender", SqlDbType.NChar);
                cmd.Parameters.Add("@HireDate", SqlDbType.Date);
                cmd.Parameters.Add("@VacationHours", SqlDbType.SmallInt);
                cmd.Parameters.Add("@SickLeaveHours", SqlDbType.SmallInt);

                Employee employee = (Employee)Util.ByteArrayToObject(binWord);
                Debug.Assert(0 == string.Compare(Util.ConvertToInt(key).ToString(), employee.UserID));

                cmd.Parameters[0].Value = employee.UserID;
                cmd.Parameters[1].Value = employee.NationalIDNumber;
                cmd.Parameters[2].Value = employee.LoginID;
                cmd.Parameters[3].Value = employee.OrganizationLevel;
                cmd.Parameters[4].Value = employee.JobTitle;
                cmd.Parameters[5].Value = employee.BirthDate;
                cmd.Parameters[6].Value = employee.MaritalStatus;
                cmd.Parameters[7].Value = employee.Gender;
                cmd.Parameters[8].Value = employee.HireDate;
                cmd.Parameters[9].Value = employee.VacationHours;
                cmd.Parameters[10].Value = employee.SickLeaveHours;

                int rows = cmd.ExecuteNonQuery();

                Console.WriteLine("{0} rows affected", rows);
            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<Cache.Word> ReadLine(Key tag, int wordsInLine)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn_;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP(");
            sb.Append(wordsInLine);
            sb.Append(@") [UserID]
                        ,[NationalIDNumber]
                        ,[LoginID]
                        ,[OrganizationLevel]
                        ,[JobTitle]
                        ,[BirthDate]
                        ,[MaritalStatus]
                        ,[Gender]
                        ,[HireDate]
                        ,[VacationHours] 
                        ,[SickLeaveHours]

                    FROM[AdventureWorks].[Cache].[Employee] ");
                        sb.Append("WHERE [UserID] >= @userId");

            cmd.CommandText = sb.ToString();
            int iTag = Util.ConvertToInt(tag);
            cmd.Parameters.Add("@userId", SqlDbType.Int);
            cmd.Parameters[0].Value = iTag;

            List<Cache.Word> words = new List<Cache.Word>();
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Employee name is {0} {1}",
                      reader["UserID"], reader["LoginID"]);

                    Employee employee = new Employee();

                    employee.UserID =                       reader.GetInt32(reader.GetOrdinal("UserID")).ToString();
                    employee.NationalIDNumber =             reader["NationalIDNumber"] as string;
                    employee.LoginID =                      reader["LoginID"] as string;
                    employee.OrganizationLevel =            reader.GetInt16(reader.GetOrdinal("OrganizationLevel"));
                    employee.JobTitle =                     reader["JobTitle"] as string;
                    employee.BirthDate =         (DateTime) reader["BirthDate"];
                    employee.MaritalStatus =                reader["MaritalStatus"] as string;
                    employee.Gender =                       reader["Gender"] as string;
                    employee.HireDate =          (DateTime) reader["HireDate"];
                    employee.VacationHours =                reader.GetInt16(reader.GetOrdinal("VacationHours"));
                    employee.SickLeaveHours =               reader.GetInt16(reader.GetOrdinal("SickLeaveHours"));

                    Word word = new Word(iTag, Util.ObjectToByteArray(employee), Word.NULL);
                    words.Add(word);
                }
                reader.Close();
            }
            catch (SqlException se)
            {
                Console.WriteLine("SQL Exception: {0}", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                cmd.Dispose();
            }

            return words;
        }
        public bool EOF(Key key)
        {
            List<Word> words = ReadLine(key, 1);
            return words.Count == 0;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (conn_.State == ConnectionState.Open)
                        conn_.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                }
            }
        }
    }
}
