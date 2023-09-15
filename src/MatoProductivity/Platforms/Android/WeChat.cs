using Com.Tencent.MM.Opensdk.Modelmsg;
using Com.Tencent.MM.Opensdk.Openapi;
using MatoProductivity.Core.WeChat;
using static Com.Tencent.MM.Opensdk.Modelmsg.WXMediaMessage;

namespace MatoProductivity.Core.Platforms.Android
{

    public class WeChat : IWeChat
    {
        private IWXAPI wxAPI;


        public string Register(string appid = "")
        {
            var activity = Platform.CurrentActivity;
            wxAPI = WXAPIFactory.CreateWXAPI(p0: activity, p1: appid, p2: true);
            var result = wxAPI.RegisterApp(appid);
            return result.ToString();
        }


        public void ShareTextTo(string text, WXScene scene)
        {
            string description = "我给你分享了一段笔记";
            var txtObj = new WXTextObject(text);
            this.ShareTo(txtObj, scene, description);
        }

        public void ShareImageTo(byte[] image, WXScene scene)
        {
            string description = "我给你分享了一个图片";
            var imageObj = new WXImageObject(image);
            this.ShareTo(imageObj, scene, description);
        }

        public void ShareTo(IMediaObject mediaObject, WXScene scene, string description)
        {
            int WXSceneValue = SendMessageToWX.Req.WXSceneSession;
            switch (scene)
            {
                case WXScene.Timeline:
                    WXSceneValue = SendMessageToWX.Req.WXSceneTimeline;
                    break;
                case WXScene.Favorite:
                    WXSceneValue = SendMessageToWX.Req.WXSceneFavorite;

                    break;
                case WXScene.SceneSession:
                    WXSceneValue = SendMessageToWX.Req.WXSceneSession;

                    break;
                default:
                    break;
            }

            var msg = new WXMediaMessage
            {
                TheMediaObject = mediaObject,
                Description = description
            };

            var req = new SendMessageToWX.Req()
            {
                Transaction = DateTime.Now.ToFileTimeUtc().ToString(),
                Message = msg,
                Scene = WXSceneValue
            };
            wxAPI.SendReq(req);

        }
    }

}
