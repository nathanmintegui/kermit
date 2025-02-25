using System.Data;

using Dapper;

using Kermit.Models;

namespace Kermit.Repositories.CustomHandlers;

public class NomeTrilhaTypeHandler : SqlMapper.TypeHandler<NomeTrilha>
{
    public override void SetValue(IDbDataParameter parameter, NomeTrilha? value)
    {
        parameter.Value = value.Value;
    }

    public override NomeTrilha Parse(object value)
    {
        return new NomeTrilha(new NonEmptyString((string)value));
    }
}
