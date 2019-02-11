﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newbe.Mahua;
using Settings;

namespace TRKS.WF.QQBot
{
    public static class Messenger
    {
        public static Dictionary<string, int> GroupCallDic = new Dictionary<string, int>();

        public static void IncreaseCallCounts(string group)
        {
            if (GroupCallDic.ContainsKey(group))
            {
                GroupCallDic[group]++;
            }
            else
            {
                GroupCallDic[group] = 1;
            }
            Task.Delay(TimeSpan.FromSeconds(60)).ContinueWith(task => GroupCallDic[group] = 0);

        }
        public static void SendDebugInfo(string content)
        {
            if (content.StartsWith("System.Threading.ThreadAbortException")) return;
            
            if (Config.Instance.QQ.IsNumber())
                SendPrivate(Config.Instance.QQ, content);
        }

        public static void SendPrivate(string qq, string content)
        {
            using (var robotSession = MahuaRobotManager.Instance.CreateSession())
            {
                var api = robotSession.MahuaApi;
                api.SendPrivateMessage(qq, content);
            }
        }

        private static Dictionary<string, string> previousMessageDic = new Dictionary<string, string>();
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SendGroup(string qq, string content)
        {
            if (previousMessageDic.ContainsKey(qq) && content == previousMessageDic[qq]) return;

            previousMessageDic[qq] = content;

            IncreaseCallCounts(qq);
            using (var robotSession = MahuaRobotManager.Instance.CreateSession())
            {
                var api = robotSession.MahuaApi;
                api.SendGroupMessage(qq, content);
            }
            Thread.Sleep(1000); //我真的很生气 为什么傻逼tencent服务器就不能让我好好地发通知 NMSL
        }

        public static void Broadcast(string content)
        {
            Task.Factory.StartNew(() =>
            {
                var count = 1;
                foreach (var group in Config.Instance.WFGroupList)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(content);
                    sb.AppendLine($"发送次序: {count}(与真实延迟了{7 * count}秒)");
                    sb.AppendLine($"如果想要获取更好的体验,请自行部署.");
                    SendGroup(group, sb.ToString().Trim());
                    count++;
                    Thread.Sleep(6000); //我真的很生气 为什么傻逼tencent服务器就不能让我好好地发通知 NMSL
                }
            }, TaskCreationOptions.LongRunning);
        }
        public static void SendHelpdoc(string group)
        {
            SendGroup(@group, @"欢迎查看机器人唯一指定帮助文档
宣传贴地址:https://warframe.love/thread-230.htm
在线最新文档:https://github.com/TRKS-Team/WFBot/blob/master/README.md (绝对最新的文档)
开源地址:https://github.com/TheRealKamisama/WFBot 
本机器人为公益项目,持续维护中.");
            if (File.Exists("data/image/帮助文档.png"))
            {
                SendGroup(@group, @"[CQ:image,file=\帮助文档.png]");
            }
            else
            {
                SendGroup(@group, @"欢迎查看破机器人的帮助文档,如有任何bug和崩溃请多多谅解.
作者: TheRealKamisama 开源地址:https://github.com/TheRealKamisama/WFBot
如果群里没有自动通知 请务必检查是否启用了通知功能
    警报: 可使用 /警报 来查询当前的所有警报.
        新警报也会自动发送到启用了通知功能的群.
    入侵: 可使用 /入侵 来查询当前的所有入侵.
        新入侵也会自动发送到启用了通知功能的群.
    突击: 可使用 /突击 来查询当前的所有突击.
        突击的奖励池为一般奖励池.
    平原时间: 可使用 /平原 来查询 地球平原 现在的时间 和 金星平原 现在的温度.
    活动:可使用 /活动 来查看目前的所有活动
    虚空商人信息: 可使用 /虚空商人 (或奸商) 来查询奸商的状态.
        如果虚空商人已经抵达将会输出所有的商品和价格, 长度较长.
    WarframeMarket 可使用 /查询 [物品名称]
        查询未开紫卡请输入: 手枪未开紫卡
    紫卡市场: 可使用 /紫卡 [武器名称]
        数据来自 WFA 紫卡市场
    地球赏金: 可使用 /地球赏金 来查询地球平原的全部赏金任务.
        可以输入数字来单一查询.
    金星赏金: 可使用 /金星赏金 来查询金星平原的全部赏金任务.
        可以输入数字来单一查询.
    裂隙: 可使用 /裂隙 来查询全部裂隙.
        目前不需要输入任何关键词了.
    遗物: 可使用 /遗物 [关键词] (eg. 后纪 s3, 前纪 B3) 来查询所有与关键词有关的遗物.
    翻译: 可使用 /翻译 [关键词]（eg. 致残突击 犀牛prime） 来 中 -> 英 / 英 -> 中 翻译.
    小小黑: 可使用 /小小黑 来查询目前小小黑的信息.
私聊管理命令:
    启用群通知: 可使用 添加群 ******* 群号 来启用[群号]对应的群的通知功能.
    禁用群通知: 可使用 删除群 ******* 群号 来禁用[群号]对应的群的通知功能.
    不启用通知功能新的任务将不会通知到群内.
贡献者: [排名不分先后]
    代码贡献者: TheRealKamisama(项目发起人 主要开发者 文档撰写) Cyl18(部分代码维护 文档修改 苦力) qiutong123(提供了翻译功能的代码)
    问题贡献者: wosiwq Aujin JJins mengpf
    捐助者列表: KonyCN
");
            }
        }

        /* 当麻理解不了下面的代码 */
        // 现在可以了
        public static void SendToGroup(this string content, string qq)
        {
            using (var robotSession = MahuaRobotManager.Instance.CreateSession())
            {
                var api = robotSession.MahuaApi;
                api.SendGroupMessage(qq, content);
            }
        }

        public static void SendToPrivate(this string content, string qq)
        {
            using (var robotSession = MahuaRobotManager.Instance.CreateSession())
            {
                var api = robotSession.MahuaApi;
                api.SendPrivateMessage(qq, content);
            }
        }
        
    }
}
