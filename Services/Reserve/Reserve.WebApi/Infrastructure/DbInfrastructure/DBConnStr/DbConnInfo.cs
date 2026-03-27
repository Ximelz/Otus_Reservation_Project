namespace ReservService
{
    public class DbConnInfo
    {
        public string Host { set; get; }
        public string Database { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public string Port { set; get; }
        public override string ToString()
        {
            if (Host == null)
                throw new ArgumentNullException("Атрибут \"Host\" не может быть null", "Ошибка создания строки подключения!");

            if (Database == null)
                throw new ArgumentNullException("Атрибут \"Database\" не может быть null", "Ошибка создания строки подключения!");

            if (Username == null)
                throw new ArgumentNullException("Атрибут \"Username\" не может быть null", "Ошибка создания строки подключения!");

            if (Password == null)
                throw new ArgumentNullException("Атрибут \"Password\" не может быть null", "Ошибка создания строки подключения!");

            if (Port == null)
                throw new ArgumentNullException("Атрибут \"Port\" не может быть null", "Ошибка создания строки подключения!");

            return $"Host={Host};Database={Database};Username={Username};Password={Password};Port={Port}";
        }
    }
}
