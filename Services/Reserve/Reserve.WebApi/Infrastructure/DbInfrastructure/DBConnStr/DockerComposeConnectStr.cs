namespace ReservService
{
    public class DockerComposeConnectStr : IConnectString
    {
        private readonly DbConnInfo dbConn;
        public DockerComposeConnectStr(string host, string path)
        {
            dbConn = ParseDockerComposeFile(host, path);
            if (dbConn == null)
                throw new Exception();
        }
        public string GetConnStr() => dbConn.ToString();

        private DbConnInfo? ParseDockerComposeFile(string host, string path)
        {
            if (!File.Exists(path))
                return null;

            var dbConn = new DbConnInfo() { Host = "localhost" };
            using (var sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                {
                    string inputStr = sr.ReadLine().Trim();
                    if (inputStr != "")
                    {
                        string[] inputSplit = inputStr.Split(':');
                        if (inputSplit.Length < 2)
                            continue;

                        if (inputSplit[0] != "container_name")
                            continue;

                        if (inputSplit[1].Trim() != host)
                            continue;

                        sr.ReadLine();

                        inputStr = sr.ReadLine().Trim();
                        inputSplit = inputStr.Split(':');
                        if (inputSplit.Length != 2)
                            return null;

                        dbConn.Username = inputSplit[1].Trim();

                        inputStr = sr.ReadLine().Trim();
                        inputSplit = inputStr.Split(':');
                        if (inputSplit.Length != 2)
                            return null;

                        dbConn.Password = inputSplit[1].Trim();

                        inputStr = sr.ReadLine().Trim();
                        inputSplit = inputStr.Split(':');
                        if (inputSplit.Length != 2)
                            return null;

                        dbConn.Database = inputSplit[1].Trim();

                        sr.ReadLine();
                        inputStr = sr.ReadLine().Trim();
                        inputSplit = inputStr.Split(':');
                        var port = inputSplit[0].Trim();
                        if (port == "")
                            return null;

                        var found = port.IndexOf("\"");
                        dbConn.Port = port.Substring(found + 1).Trim();
                        return dbConn;
                    }
                }
            }

            return null;
        }
    }
}
