﻿using Newbe.Mahua;

namespace TRKS.WF.QQBot
{
    /// <summary>
    /// 本插件的基本信息
    /// </summary>
    public class PluginInfo : IPluginInfo
    {
        /// <summary>
        /// 版本号，建议采用 主版本.次版本.修订号 的形式
        /// </summary>
        public string Version { get; set; } = "123132132";

        /// <summary>
        /// 插件名称
        /// </summary>

        public string Name { get; set; } = "WFBot";

        /// <summary>
        /// 作者名称
        /// </summary>
        public string Author { get; set; } = "TheRealKamisama";

        /// <summary>
        /// 插件Id，用于唯一标识插件产品的Id，至少包含 AAA.BBB.CCC 三个部分
        /// </summary>
        public string Id { get; set; } = "TRKS.WF.QQBot";

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; } = "一个Warframe的QQ机器人";
    }
}