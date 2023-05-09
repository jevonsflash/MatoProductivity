using Abp.Dependency;

namespace MatoProductivity.Core
{
    public interface IWeChat : ISingletonDependency
    {
        string Register(string appid = "");
        void ShareImageTo(byte[] image, WXScene scene);
        void ShareTextTo(string text, WXScene scene);
    }
}