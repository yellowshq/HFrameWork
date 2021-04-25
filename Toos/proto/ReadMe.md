### 协议生成规则
**********
* 文件名要作为proto的packageName 
* MsgName前面一行要加//GenProtoMap ID = xxxx
* 例如（Test.proto）:
  ```
    syntax = "proto3";
    package Test;

    //GenProtoMap ID = 10001001
    message Person{
        string Name = 1;
        int32 age = 2;
        repeated string emails = 3;
        repeated PhoneNumber phones = 4;
    }
  ```
---
 * 消息号定义：
 * 1.消息为10进制8位数
 * 2.前四位为功能编号（1000~9999）
 * 3.第五位为来源（1:SC   2:CS  3:SS  4:CC）
 * 4.第六位为占位符(目前使用0占位)
 * 5.后两位为具体功能来源的消息编号（01~99）
 * ==================================================
 * 例如
 * 消息号：10002001
 * 前四位   1000     xx功能
 * 第五位   2      （1:SC   2:CS  3:SS  4:CC 5:CW 6:WC）
 * 第六位   0    占位符
 * 客户端发送到服务器Req前缀-第五位2
 * 服务器发送到客户端Res前缀-第五位1
 * ==================================================
