using System.Text.Json;

namespace ReservService
{
    public class JsonConnectStr : IConnectString
    {
        private readonly DbConnInfo? dbConn;
        public JsonConnectStr(string path)
        {
            if (!File.Exists(path))
                throw new Exception("Файл не найден!");
            string jsonStr = "";

            using (var sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                    jsonStr += sr.ReadLine();
            }

            dbConn = JsonSerializer.Deserialize<DbConnInfo>(jsonStr);

            if (dbConn == null)
                throw new ArgumentNullException("Неверный Json", "Ошибка десериалзации json!");
        }
        public string GetConnStr() => dbConn.ToString();
    }
}
