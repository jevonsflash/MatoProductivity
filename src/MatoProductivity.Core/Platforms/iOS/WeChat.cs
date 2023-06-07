using MatoProductivity.Core.WeChat;
using WXApi_Api;
using WXApiObject_Api;
using WXApiObject_Scene = WXApiObject_Structs.WXScene;

namespace MatoProductivity.Core.Platforms.Android
{
    public class WeChat : IWeChat
    {
        public string Register(string appid = "")
        {
            var result = WXApi.RegisterApp(appid: appid, universalLink: "");
            return result.ToString();
        }

        public void _ShareTextTo(string msg, WXScene scene, string description)
        {
            int WXSceneValue = (int)WXApiObject_Scene.Session;
            switch (scene)
            {
                case WXScene.Timeline:
                    WXSceneValue = (int)WXApiObject_Scene.Timeline;
                    break;
                case WXScene.Favorite:
                    WXSceneValue = (int)WXApiObject_Scene.Favorite;

                    break;
                case WXScene.SceneSession:
                    WXSceneValue = (int)WXApiObject_Scene.Session;

                    break;
                default:
                    break;
            }



            var req = new SendMessageToWXReq()
            {
                Scene = WXSceneValue,

                Text = msg,
                BText = true,
            };
            WXApi.SendReq(req, isOK =>
            {

            });

        }

        public void ShareImageTo(byte[] image, WXScene scene)
        {
            throw new NotImplementedException();
        }

        public void ShareTextTo(string text, WXScene scene)
        {
            this._ShareTextTo(text, scene, string.Empty);
        }
    }
}
