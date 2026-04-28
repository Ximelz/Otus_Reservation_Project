namespace ReservService
{
    public class DBConnStrStrategy
    {
        private readonly IConnectString dbConn;
        public DBConnStrStrategy(string type)
        {
            if (type == "docker-compose")
            {
                dbConn = new DockerComposeConnectStr("postgres_reserv_db", "..\\..\\..\\docker-compose.yml");
            }
            else if (type == "local")
            {
                dbConn = new JsonConnectStr("..\\..\\..\\DBConn.json");
            }
            else
                throw new ArgumentException("Неверный тип базы данных");
            
        }
        public IConnectString GetConnStr() => dbConn;
    }
}
