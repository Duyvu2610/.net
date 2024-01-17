namespace Common.Config;
public class DotenvConfig
{
  public static void Load(string file = ".env")
  {
    string path = Path.Join(Directory.GetCurrentDirectory(), file);
    if (!File.Exists(path)) {
      return;
    }

    foreach (var line in File.ReadAllLines(path))
    {
      var parts = line.Split(
          '=',
          StringSplitOptions.RemoveEmptyEntries);

      if (parts.Length != 2)
        continue;

      Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
    }
  }
}