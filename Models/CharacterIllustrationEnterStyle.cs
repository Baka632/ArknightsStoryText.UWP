﻿//原项目：https://github.com/ArknightsResources/Utility
//为了兼容 Windows 10 1703及以下版本， 我们无法直接引用 NuGet 包，所以只能复制源代码了（

namespace ArknightsResources.Stories.Models
{
    /// <summary>
    /// 表示干员立绘进入方式的枚举
    /// </summary>
    public enum CharacterIllustrationEnterStyle
    {
        /// <summary>
        /// 无进入效果
        /// </summary>
        None,
        /// <summary>
        /// 从左进入
        /// </summary>
        Left,
        /// <summary>
        /// 从右进入
        /// </summary>
        Right
    }
}
