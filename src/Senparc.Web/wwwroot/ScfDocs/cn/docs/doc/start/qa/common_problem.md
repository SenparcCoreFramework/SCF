# SCF常见问题

> 如何修改数据库配置

![Image Text](/start/images/modify_database_connectstring.png)

> .net命令dotnet ef执行报错

错误信息：
```
Could not execute because the specified command or file was not found.
Possible reasons for this include:
  * You misspelled a built-in dotnet command.
  * You intended to execute a .NET Core program, but dotnet-ef does not exist.
  * You intended to run a global tool, but a dotnet-prefixed executable with this name could not be found on the PATH.
```

解决方案：
使用的命令为
```
dotnet ef database update
```
查看当前dot版本为3.0

解决办法：

需要更新dotnet tool，使用的命令为：
```
dotnet tool update --global dotnet-ef --version 3.0.0-preview7.19362.6
```
执行此命令之后再更新数据库就执行成功了。

[参考地址](https://blog.csdn.net/topdeveloperr/article/details/101282099)

> SCF如何调试

[参考地址](https://www.cnblogs.com/szw/p/debug-remote-source-code.html)

> 后台UI框架

[参考地址](https://colorlib.com/polygon/gentelella/icons.html)