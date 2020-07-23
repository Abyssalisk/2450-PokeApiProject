using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PokemonSimulator
{
    class DBData1
    {
        /// <summary>
        /// Must be a member or will not be included in serialization.
        /// </summary>
        public int versionID = 1;
        public int PokemonIDForProfilePicture { get; }
        public DBData1(int pokeIDForProfile) => PokemonIDForProfilePicture = pokeIDForProfile;
    }
    class DBData2
    {
        /// <summary>
        /// Must be a member or will not be included in serialization.
        /// </summary>
        public int versionID = 2;
        public int PokemonIDForProfilePicture { get; set; }
        public int[][] CustomLeaderboardsAndIDs { get; set; } //index one is the custom leaderboard index, index two is the list of UserIDs in that leaderboard.
        public DBData2(int pokeIDForProfile, int[][] customLeaderboards)
        {
            PokemonIDForProfilePicture = pokeIDForProfile; 
            CustomLeaderboardsAndIDs = customLeaderboards; 
        }
        public static explicit operator DBData2(DBData1 old)
        {
            return new DBData2(old.PokemonIDForProfilePicture, new int[0][]);
        }
    }
    class DBIngest
    {
        public static DBData2 Ingest(string json) //update the return type of this method to be whatever the latest version of the DBData class there is.
        {
            //read the version id value of the json dynamically with a switch statment.
            //based on the version id, create a DBData# object where the number is the version number of the json.
            //explicitly convert that new object to the newest DBData data type.
            //mock up
            JObject js = new JObject(json);
            switch (js["versionID"].Value<int>())
            {
                case 1:
                    return (DBData2)JsonConvert.DeserializeObject<DBData1>(json);
                case 2:
                    return JsonConvert.DeserializeObject<DBData2>(json);
                default:
                    throw new InvalidCastException("Error: The provided json does not conform to a known DBData type.");
            }
        }
    }
}