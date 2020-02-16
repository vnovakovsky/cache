using Cache;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace ClientApp
{
    class DatabaseStorage<Key, Value> : Cache.IStorage<Key> where Key : unmanaged, IComparable
    {
        SqlConnection connection_;
        internal DatabaseStorage()
        {
            string dataSourceString = ConfigurationManager.AppSettings["DataSourceString"];
            connection_ = new SqlConnection(
            dataSourceString); //"server=WOOD;uid=CacheUser;pwd=CacheUser;database=AdventureWorks");
            connection_.Open();
        }
        // not used
        public byte[] ReadWord(Key key)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(Key key, byte[] binWord)
        {
            string sqlString =
               @"UPDATE [Cache].[Employee]
                   SET 
                      [NationalIDNumber] = @NationalIDNumber
                      ,[LoginID] = @LoginID
                      ,[OrganizationLevel] = @OrganizationLevel
                      ,[JobTitle] = @JobTitle
                      ,[BirthDate] = @BirthDate
                      ,[MaritalStatus] = @MaritalStatus
                      ,[Gender] = @Gender
                      ,[HireDate] = @HireDate
                      ,[VacationHours] = @VacationHours
                      ,[SickLeaveHours] = @SickLeaveHours
                 WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(sqlString, connection_);
            try
            {
                cmd.CommandType = CommandType.Text;

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

                Logger.LogInfo(string.Format("update: {0} rows affected", rows));
            }
            catch (SqlException se)
            {
                Logger.LogError(se);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public List<Cache.Word> ReadLine(Key tag, int wordsInLine)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection_;
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

                    Word word = new Word(int.Parse(employee.UserID), Util.ObjectToByteArray(employee), Word.NULL);
                    words.Add(word);
                    if(words[0].Tag != iTag) // requested tag should be present - it goes first
                    {
                        words.Clear();
                        return words; // if requested tag doesn't exist then return empty list
                    }
                }
                reader.Close();
            }
            catch (SqlException se)
            {
                Logger.LogError(se);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                cmd.Dispose();
            }

            return words;
        }
        public int GetMaxRecordLength()
        {
            SqlCommand command = new SqlCommand(
            @"SELECT max_record_size_in_bytes 
              FROM sys.dm_db_index_physical_stats(
                                                DB_ID(N'AdventureWorks')
                                              , OBJECT_ID(N'[AdventureWorks].[Cache].[Employee]')
                                              , NULL, NULL, 'DETAILED')", connection_);
            int maxRecordLength = (int)command.ExecuteScalar();
            return maxRecordLength;
        }
        public bool EOF(Key key)
        {
            return key.CompareTo(MaxKey) > 0;
        }
        private Key MaxKey
        {
            get
            {
                SqlCommand command = new SqlCommand(
                @"SELECT max([UserID])
                  FROM [AdventureWorks].[Cache].[Employee]", connection_);
                int maxUserID = (int)command.ExecuteScalar();
                return Util.Convert<Key, int>(maxUserID);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (connection_.State == ConnectionState.Open)
                        connection_.Close();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }
    }
}
