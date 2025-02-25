using System.Data;

using Dapper;

using Kermit.Models;

namespace Kermit.Repositories.CustomHandlers;

public class NomeEdicaoTypeHandler : SqlMapper.TypeHandler<NomeEdicao>
{
    public override void SetValue(IDbDataParameter parameter, NomeEdicao? value)
    {
        parameter.Value = value.Value;
    }

    public override NomeEdicao? Parse(object value)
    {
        return new NomeEdicao(new NonEmptyString((string)value));
    }
}
