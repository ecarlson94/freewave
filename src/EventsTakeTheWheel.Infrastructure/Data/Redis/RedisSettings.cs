namespace EventsTakeTheWheel.Infrastructure.Data.Redis;

public abstract class RedisSettings {
  public abstract string Endpoint { get; }
  public abstract string Port { get; }
  public abstract string Ssl { get; }
  public abstract string Password { get; }

  public string ConnectionString =>
      $"{Endpoint}:{Port},ssl={Ssl}{(string.IsNullOrEmpty(Password) ? "" : $",password={Password}")}";
  public string ConnectionUri =>
        $"redis{(Ssl == "true" ? "s" : "")}://:{(string.IsNullOrEmpty(Password) ? "" : $"{Password}@")}{Endpoint}:{Port}";
}
