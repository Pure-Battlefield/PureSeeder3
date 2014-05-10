namespace PureSeeder.Core.Context
{
    public interface IDataContextUpdater
    {
        void Update(IDataContext context, string pageData);
    }
}