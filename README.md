这是一位Unity初学者在学习过程中自主开发的UI框架。经过本人的多次GameJam打磨与优化，框架逐步完善。（虽然目前称之为UI框架，但核心实际上是一个面板基类，在未来中将持续更新与扩展

示例项目演示视频：https://www.bilibili.com/video/BV1eSH9eREB2/

目前主要内容为:
1. Scripts/UI/BasePanel.cs
2. Scripts/UI/UIManager.cs

特点：
1. 所有的面板都采用单例模式，并包含在场景中且处于失活状态。
2. 项目还包含示例项目（示例项目的包体做了删改，下载完包体后项目大小仅有100MB左右）和示例面板，便于学习和参考。

需要注意的：
1. 所有的IBasePanel都应该作为UIManager的子物体，以便在未激活的情况下执行Init方法来进行初始化。
2. CallBack方法（面板的淡入淡出逻辑）的默认实现使用了dotween插件，如果您的项目没装的话，你可以装一个dotween。或者直接把方法删掉，然后自己重新实现面板淡入淡出的方法。
