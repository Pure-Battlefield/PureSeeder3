using System.Threading.Tasks;
using PureSeeder.Core.Context;

namespace PureSeeder.Core.ServerManagement
{
    public interface IUpdateServerIds
    {
        Task Update(IDataContext context, bool updateAll = false);
    }
}