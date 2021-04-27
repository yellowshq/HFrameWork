# HFrameWork 一个基本的unity3D的前端框架
主要将xlua、uniRx、uniTask、lua-protobuf、addressables、unityCCD整合在一起，写的一套框架。
## 简介
1. 资源管理
    - 使用unity的addresables做资源管理，将其资源做分组处理并通过脚本自动设置group（Addon/AutoSetGroup）
    - 通过AssetCacheManager做资源加载和卸载的整体管理
2. 热更新
    - 使用unity的CCD服务做资源热更新
    - 接入xlua来在lua侧开发逻辑以便更新
3. 网络
    - socket TCP连接
    - 服务器采用Golang作为服务器语言
    - lua侧和C#侧均已封装网络接口
4.UI框架(暂未处理)
5.工具 
    - 协议ID对应生成json
    - 表格生成对应lua（暂未处理）
