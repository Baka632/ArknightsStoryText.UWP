<p align="center">
<img src="./assets/Icon.png" width="100px"/>
</p>

<div align="center">

# Arknights Story Text - UWP
一个基于 UWP 的明日方舟纯文本剧情工具

</div>

## 最低支持平台
~~Windows 10 1607 (14393)~~（仅 4.2.0.0 以下版本）

Windows 10 1703 (15063) - 4.2.0.0 及以上版本

> 升级原因：为了使用 [WinUI 2](https://github.com/microsoft/microsoft-ui-xaml) 库。
>
>WinUI 2 库能让此程序在新版本系统上获得新外观，在旧版本系统上使用新版本的控件。

## 构建需求
- Visual Studio 2022 及以上
- .NET 7 SDK（为了使用最新的 C# 语言版本）
- Windows 10 SDK (至少为 16299，因为需要生成 ARM64 版本的程序)
    - 需安装 Windows SDK for UWP Managed Apps

## 贡献者须知
为了兼容 ARM64 架构，我们将最低版本设置为了 Windows 10 1709 (Build 16299)。

但是，为了兼容 Windows 10 Mobile 设备，我们不应在不添加兼容性检测的情况下，使用任何不支持 Windows 10 1703 (Build 15063) 的 API

另外，若引用了 .NET Standard 2.0 库，在部署到 Windows 10 Mobile 设备时，应使用 `Release` 配置来进行部署或安装。

## 使用的开源项目
[ArknightsResources/Utility](https://github.com/ArknightsResources/Utility) (MIT 许可证，以源代码形式使用)

## 许可
MIT 许可证