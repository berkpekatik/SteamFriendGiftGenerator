using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SteamFriendGiftGenerator.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly WebClient _webclient;

        public bool error = false;

        public string giftDate = "";
        public string gameName = "";
        public string gameExplain = "";
        public string giftProfile = "";
        public string giftAvatar = "";
        public string gift_message = "";
        public string signature = "";
        public string gamePic = "";
        public string gifterName = "";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _webclient = new WebClient();
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public void OnPost()
        {
            var steamID = Request.Form["steamID"].ToString();
            var appID = Request.Form["appID"].ToString();

            gift_message = Request.Form["note"].ToString();
            signature = Request.Form["senderName"].ToString();
            giftDate = $"{DateTime.Now.ToString("dd ddd")}";

            var user = JsonConvert.DeserializeObject<PlayerModel>(_webclient.DownloadString("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=088F8DE259B7EFA9BE71D0CCADB7EC1F&steamids=" + steamID));
            var game = JsonConvert.DeserializeObject<JObject>(_webclient.DownloadString("https://store.steampowered.com/api/appdetails?appids=" + appID));
            if (user != null && user.response.players != null && user.response.players.Count > 0)
            {
                giftProfile = user.response.players.First().profileurl;
                giftAvatar = user.response.players.First().avatar;
                gifterName = user.response.players.First().personaname;
            }
            if (game != null)
            {
                var gameModel = JsonConvert.DeserializeObject<GameModel>(game[appID].ToString());
                if (gameModel.success)
                {
                    gameExplain = gameModel.data.short_description;
                    gamePic = gameModel.data.header_image;
                    gameName = gameModel.data.name;
                }
            }
            error = true;
        }
    }
}