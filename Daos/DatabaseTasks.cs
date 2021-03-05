using System;
using System.Collections.Generic;
using dto = aspet.Models;
using aspet.Utils;
using MySql.Data.MySqlClient;

namespace aspet.Daos
{
    public class DatabaseTasks : ITasks
    {
        private MysqlDatabase database;

        public DatabaseTasks(MysqlDatabase db)
        {
            this.database = db;
        }

        public void Archive(string email, int id)
        {
            using (var cmd = this.database.Connection.CreateCommand() as MySqlCommand)
            {
                cmd.CommandText = @"UPDATE Tasks SET Completed = STR_TO_DATE(@Date, '%Y/%m/%d') WHERE TaskId = @TaskId;";
                cmd.CommandText = @"UPDATE Tasks SET Archived = STR_TO_DATE(@Date, '%Y/%m/%d') WHERE TaskId = @TaskId;";
                cmd.Parameters.AddWithValue("@TaskId", id);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.ExecuteNonQuery();
            }
        }

        public void Complete(string email, int id)
        {
            using (var cmd = this.database.Connection.CreateCommand() as MySqlCommand)
            {
                cmd.CommandText = @"UPDATE Tasks SET Completed = STR_TO_DATE(@Date, '%Y/%m/%d') WHERE TaskId = @TaskId;";
                cmd.Parameters.AddWithValue("@TaskId", id);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.ExecuteNonQuery();
            }
        }

        public void Create(string email, string text)
        {
            using (var cmd = this.database.Connection.CreateCommand() as MySqlCommand)
            {
                cmd.CommandText = @"INSERT INTO Tasks(Email, Text, Created) VALUES (@Email,@Text,STR_TO_DATE(@Date, '%Y/%m/%d'));";
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.ExecuteNonQuery();
            }
        }

        public void Incomplete(string email, int id)
        {
            using (var cmd = this.database.Connection.CreateCommand() as MySqlCommand)
            {
                cmd.CommandText = @"UPDATE Tasks SET Completed = NULL WHERE TaskId = @TaskId;";
                cmd.Parameters.AddWithValue("@TaskId", id);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.ExecuteNonQuery();
            }
        }

        public List<dto.Task> List(string email)
        {
            var ret = new List<dto.Task>();
            using (var cmd = this.database.Connection.CreateCommand() as MySqlCommand)
            {
                cmd.CommandText = @"SELECT TaskId, Email, Text, Completed FROM Tasks WHERE Archived IS NULL";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var t = new dto.Task()
                    {
                        TaskId = reader.GetFieldValue<int>(0),
                        Email = reader.GetFieldValue<string>(1),
                        Text = reader.GetFieldValue<string>(2)
                    };
                    if (!reader.IsDBNull(3))
                        t.Completed = reader.GetFieldValue<DateTime>(3);

                    ret.Add(t);
                }
            }

            return ret;
        }
    }
}
