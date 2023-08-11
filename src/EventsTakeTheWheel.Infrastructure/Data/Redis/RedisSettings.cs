namespace EventsTakeTheWheel.Infrastructure.Data.Redis;

public class RedisSettings
{
    public string Endpoint { get; set; } = "localhost";
    public int Port { get; set; } = 6379;
    public bool Ssl { get; set; } = false;
    public string Password { get; set; } = "";

    public string ConnectionString =>
        $"{Endpoint}:{Port},ssl={Ssl.ToString().ToLower()}{(string.IsNullOrEmpty(Password) ? "" : $",password={Password}")}";
    public string ConnectionUri =>
        $"redis{(Ssl ? "s" : "")}://:{(string.IsNullOrEmpty(Password) ? "" : $"{Password}@")}{Endpoint}:{Port}";
}
