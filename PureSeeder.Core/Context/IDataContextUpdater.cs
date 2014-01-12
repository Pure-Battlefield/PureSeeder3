namespace PureSeeder.Core.Context
{
    public interface IDataContextUpdater
    {
        void UpdateContextData(IDataContext context, string pageData);
    }
}