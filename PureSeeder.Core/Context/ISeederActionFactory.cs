namespace PureSeeder.Core.Context
{
    public interface ISeederActionFactory
    {
        SeederAction GetAction(IDataContext context);
    }
}