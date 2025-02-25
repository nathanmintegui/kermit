using System.Data;

using Dapper;

using Kermit.Models;

namespace Kermit.Repositories.CustomHandlers;

public class EdicaoIdTypeHandler : SqlMapper.TypeHandler<EdicaoId>
{
    public override void SetValue(IDbDataParameter parameter, EdicaoId value)
    {
        parameter.Value = value.Valor;
    }

    public override EdicaoId Parse(object value)
    {
        return EdicaoId.Create((int)value);
    }
}
