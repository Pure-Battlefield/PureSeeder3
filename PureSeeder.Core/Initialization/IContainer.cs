namespace PureSeeder.Core.Initialization
{
    public interface IContainer
    {
        T Resolve<T>();
    }
}