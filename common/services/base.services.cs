using Common.Config;

namespace Common.Services;

public class BaseService {
  protected PostgresFactory pgFactory;

  public BaseService (PostgresFactory postgresFactory) {
    pgFactory = postgresFactory;
  }
}