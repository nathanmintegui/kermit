using System.Data;

using Dapper;

using Kermit.Models;

namespace Kermit.Repositories.CustomHandlers;

public class TrilhaIdTypeHandler : SqlMapper.TypeHandler<TrilhaId>
{
    public override void SetValue(IDbDataParameter parameter, TrilhaId value)
    {
        parameter.Value = value.Valor;
    }

    public override TrilhaId Parse(object value)
    {
        return TrilhaId.Create((int)value);
    }
}
