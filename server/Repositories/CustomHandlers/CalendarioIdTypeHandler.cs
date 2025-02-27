using System.Data;

using Dapper;

using Kermit.Models;

namespace Kermit.Repositories.CustomHandlers;

public class CalendarioIdTypeHandler : SqlMapper.TypeHandler<CalendarioId>
{
    public override void SetValue(IDbDataParameter parameter, CalendarioId value)
    {
        parameter.Value = value.Valor;
    }

    public override CalendarioId Parse(object value)
    {
        return CalendarioId.Create((Guid)value);
    }
}
