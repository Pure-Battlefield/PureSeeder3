namespace PureSeeder.Core.Context
{
    public interface IPlayerStatusGetter
    {
        PlayerStatus GetPlayerStatus(IDataContext context);
    }
}