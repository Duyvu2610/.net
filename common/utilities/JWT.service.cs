using Common.Config;
using Common.Services;
using hrm_server.entities;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.EntityFrameworkCore;

public class JWTService : BaseService
{
  IJwtAlgorithm algorithm;
  IJsonSerializer serializer;
  IBase64UrlEncoder urlEncoder;
  IJwtEncoder encoder;
  IJwtDecoder decoder;
  IJwtValidator validator;
  IDateTimeProvider provider;
  string? key;

  public JWTService(PostgresFactory postgresFactory) : base(postgresFactory)
  {
    algorithm = new HMACSHA256Algorithm();
    serializer = new JsonNetSerializer();
    urlEncoder = new JwtBase64UrlEncoder();
    provider = new UtcDateTimeProvider();
    validator = new JwtValidator(serializer, provider);
    encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
    decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
    key = Environment.GetEnvironmentVariable("JWT_SCERET");
  }

  public string getToken(JWTPayload payload)
  {
    return encoder.Encode(payload, key);
  }

  public JWTPayload verifyToken(string jwt)
  {
    IDictionary<string, object> json = decoder.DecodeToObject(jwt, key) as IDictionary<string, object>;
    JWTPayload payload = new JWTPayload(
      int.Parse(json["id"].ToString()),
      (string)json["email"],
      (string)json["role"]
    );
    payload.createdAt = (DateTime)json["createdAt"];
    return payload;
  }

  public RefreshToken? getRefresh(string jwt)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    RefreshToken? rf = pgContext.RefreshTokens.FirstOrDefault(rf => rf.Refresh == jwt);
    return rf;
  }

  public string createRefresh(JWTPayload payload)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    RefreshToken? rsToken = pgContext.RefreshTokens
      .FirstOrDefault(rs => rs.UserId == payload.id && rs.IsActive == true);
    if(rsToken != null) {
      return rsToken.Refresh;
    }
    string jwt = getToken(payload);
    RefreshToken token = new RefreshToken
    {
      Refresh = jwt,
      CreatedAt = payload.createdAt,
      UserId = payload.id,
      IsActive = true
    };

    pgContext.RefreshTokens.Add(token);
    pgContext.SaveChanges();
    return jwt;
  }

  public string create(JWTPayload payload)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    string jwt = getToken(payload);
    unActiveTokens(payload.id);
    Tokens token = new Tokens
    {
      Token = jwt,
      CreatedAt = payload.createdAt,
      UserId = payload.id,
      IsActive = true
    };

    pgContext.Tokens.Add(token);
    pgContext.SaveChanges();
    return jwt;
  }

  public bool isExpried(int userId, string accessToken)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    Tokens? token = pgContext.Tokens
      .Where(t => t.UserId == userId)
      .Where(t => t.Token == accessToken)
      .Where(t => t.IsActive == true)
      .FirstOrDefault();

    if (token == null) return true;

    long minutes = ((long)(DateTime.Now - token.CreatedAt).TotalMilliseconds) / 1000 / 60;

    return minutes > 30;
  }

  public void unActiveTokens(int userId)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    List<Tokens>? tokens = pgContext.Tokens
      .Where(t => t.UserId == userId)
      .Where(t => t.IsActive == true)
      .ToList();
    DateTime currentTime = DateTime.Now;
    tokens.ForEach(token =>
    {
      token.IsActive = false;
      token.UpdatedAt = currentTime;
    });

    pgContext.Tokens.UpdateRange(tokens);
    pgContext.SaveChanges();
  }
}