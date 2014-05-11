using System;
using System.Threading.Tasks;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Settings;

namespace PureSeeder.Core.Context
{
    public interface IDataContext
    {
        /// <summary>
        /// Data related to the current session
        /// </summary>
        SessionData Session { get; }
        /// <summary>
        /// Any user-changeable settings
        /// </summary>
        BindableSettings Settings { get; }
        
        /// <summary>
        /// Exports settings to a json file
        /// </summary>
        /// <param name="filename"></param>
        void ExportSettings(string filename);

        /// <summary>
        /// Imports settings from a json file
        /// </summary>
        /// <param name="filename"></param>
        void ImportSettings(string filename);


        /// <summary>
        /// Update current status with the given page data
        /// </summary>
        /// <param name="pageData">Raw page data</param>
        void UpdateContextWithBrowserPage(string pageData); 

        /// <summary>
        /// Update the statuses of all the servers in the list
        /// </summary>
        Task UpdateServerStatuses();
        
        /// <summary>
        /// Event fired when UpdateContextWithBrowserPage is complete
        /// </summary>
        event ContextUpdatedHandler OnContextUpdate;
        /// <summary>
        /// Event fired when Hang Protection is invoked
        /// </summary>

        /// <summary>
        /// Update the context in any necessary ways when a server is joined
        /// </summary>
        void JoinServer();

        /// <summary>
        /// Checks if BF is currently seeding
        /// </summary>
        /// <returns>True if seeding, else false</returns>
        bool IsSeeding();

        /// <summary>
        /// Checks whether the current user is the correct user
        /// </summary>
        /// <returns></returns>
        UserStatus GetUserStatus();

        // Deprecated
//        ResultReason<ShouldNotSeedReason> ShouldSeed();
//        ResultReason<KickReason> ShouldKick();

        //void StopGame(); Deprecated


        PlayerStatus GetPlayerStatus();
    }
}