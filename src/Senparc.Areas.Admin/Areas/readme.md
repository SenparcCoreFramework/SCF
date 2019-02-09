## 调试说明

Razor Page 的独立库需要编译成 dll 后被 Senparc.Web 项目引用才能生效。

因此，如果需要快速调试 .cshtml 中的页面内容（避免每次都调试），可以将此文件夹（Areas）整体移动到 Senparc.Web 根目录下。