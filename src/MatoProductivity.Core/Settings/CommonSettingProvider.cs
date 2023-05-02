using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Configuration;
using Abp.Localization;

namespace MatoProductivity.Core.Settings
{
    internal class CommonSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                   {
                       new SettingDefinition(CommonSettingNames.DatabaseVersion, "1999-01-01", L("数据库版本"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsSleepModeOn, "false", L("睡眠模式开关"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsAutoLrc, "true", L("是否自动歌词"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsAutoOffset, "false", L("是否自动滚动"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsStopWhenTerminate, "false", L("离开后关闭"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.TimingOffValue, "1200", L("倒计时"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.BreakPointMusicIndex, "0", L("歌曲上次播放位置"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsRepeat, "false", L("是否循环"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsRepeatOne, "false", L("是否单曲循环"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsShuffle, "false", L("是否随机播放"), scopes: SettingScopes.All),
                       new SettingDefinition(CommonSettingNames.IsHideQueueButton, "false", L("是否隐藏正在播放页面底端的队列按钮"), scopes: SettingScopes.All),
                   };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }

}
