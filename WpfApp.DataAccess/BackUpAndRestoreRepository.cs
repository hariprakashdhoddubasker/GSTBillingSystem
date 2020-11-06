using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading.Tasks;

namespace WpfApp.DataAccess
{
    public class BackUpAndRestoreRepository : IBackUpAndRestoreRepository
    {
        private string constring = ConfigurationManager.ConnectionStrings["WpfAppDB"].ConnectionString;
        public async Task<bool> BackUp(string folderPath)
        {
            await Task.Run(() =>
            {
                string file = $"{folderPath}\\AppBackup.sql";
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(file);
                            conn.Close();
                        }
                    }
                }
            });
            return true;
        }
        public async Task<bool> Restore(string filePath)
        {
            await Task.Run(() =>
            {
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ImportFromFile(filePath);
                            conn.Close();
                        }
                    }
                }
            });
            return true;
        }

    }
}
