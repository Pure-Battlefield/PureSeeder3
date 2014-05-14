using System.Threading.Tasks;

namespace PureSeeder.Core.Context
{
    public interface ISeederActionFactory
    {
        Task<SeederAction> GetAction(IDataContext context);
    }
}