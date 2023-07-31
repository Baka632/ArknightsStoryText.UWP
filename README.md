<p align="center">
<img src="./assets/Icon.png" width="100px"/>
</p>

<div align="center">

# Arknights Story Text - UWP
一个基于 UWP 的明日方舟纯文本剧情工具

</div>

## 最低支持平台
Windows 10 1607 (14393)

## 构建需求
- Visual Studio 2022 及以上
- .NET 7 SDK（为了使用最新的 C# 语言版本）
- Windows 10 SDK (至少为 14393)
    - 需安装 Windows SDK for UWP Managed Apps

## 贡献者须知
为了兼容 ARM64 架构，我们将最低版本设置为了 Windows 10 1709 (Build 16299)。

但是，为了兼容 Windows 10 Mobile 设备，我们不应：
- 引用任何 .NET Standard 1.4 以上的库（不含 .NET Standard 1.4）
- 在不添加兼容性检测的情况下，使用任何不支持 Windows 10 1607 (Build 14393) 的 API

## 使用的开源项目
[ArknightsResources/Utility](https://github.com/ArknightsResources/Utility) (MIT 许可证，以源代码形式使用)

## 许可
MIT 许可证