using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace backend.Infrastructure
{
    public class DalHelper()
    {
        private static SQLiteConnection sqliteConnection;

        // public DalHelper(){}

        private static SQLiteConnection DbConnection(){
            sqliteConnection = new SQLiteConnection("Data Source=C:\\Users\\Fabricio\\Codes\\WebChat\\xDataBase\\Users.sqlite");\
            sqliteConnection.Open();
            return sqliteConnection;
        }

        public static void CriarBancoSQLite(){
            try{
                SQLiteConnection.CreateFile(@"C:\Users\Fabricio\Codes\WebChat\xDataBase\Users.sqlite");
            }
            catch{
                throw;
            }
        }
        
        public static void CriarTabelaSQLite(){
            try{
                using(var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Users(id int, Name Varchar(50), number Varchar(50), password Varchar(50))";
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public static DataTable GetUsers(){
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try{
                using (var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "SELECT * FROM Users";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex){
                throw ex;
            }
        }

        public static DataTable GetUser(int id){
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "SELECT * FROM Users Where Id=" + id;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(User user){
            try{
                using (var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "INSERT INTO Clientes(id, Name, number, password) values (@id, @nome, @number, @email)";
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Number", user.Number);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public static void Update(User user){
            try {
                using (var cmd = new SQLiteCommand(DbConnection())){
                    if (user.Id != null){
                        cmd.CommandText = "UPDATE Users SET Name=@Name, Number=@Number, Password=@Password WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@Number", user.Number);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex){
                throw ex;
            }
        }

        public static void Delete(int Id){
            try {
                using(var cmd = new SQLiteCommand(DbConnection())){
                    cmd.CommandText="DELETE FROM Users WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }

    }
}