bower_components 文件夹中是 使用包管理器：Bower 下载的文件目录。

lib 文件说明：
lib 文件为 项目使用的 UI 框架 以及其他插件。可在Teams [盛派网络-开发/常规/文件/模板 ] 中下载： 下载 文件：WB04HF123.zip。本项目使用的是 WB04HF123/backend-jquery 模板。

images 文件说明：
images/{Areas}：images文件下对应的是 Areas。 

css 文件说明：
css/{Areas}：css文件下对应的是 Areas。 

js 文件说明：
js/{Areas}/{Controller}：
    js文件下对应的是 Areas ,每个Areas 下的Controller 对应一个文件夹，里面放的是对应视图的js 文件。每个视图对应一个 js 文件。 
js/{Areas}/base 说明 ：
    base 中含有 封装的 Ajax.js 文件 ，js 枚举 enum.js ，base.js 中包含被封装过的 alert、confirm （基于 sweetAlert.js）以及常用格式校验：身份证、手机号等。
js/{Areas}/base.min.js：
    base 中js（除 Plugins）中所有js enum.js ，base.js，ajax.js 的压缩版 js ；如果后续有其他文件 可前往 bundleconfig.json 文件中对应添加。

**注意**
 1、js/User 使用 Vue.js 框架， 每建立一个视图 则必须对应建立一个 js，并且写上 实例化 代码。否则页面无法显示。
 2、每个视图对应对应一个js 文件，基本在页面上引用，而非直接在 母版页应用。
 3、在引用 js 、css 文件时，分为线上和线下。线上引用 .min 文件，本地则引用源文件。尤其是base 文件中js文件 只对应一个 base.min.js 文件。
 4、如需要定义枚举，则直接在 enum.js 中 定义。
 5、js、css 文件压缩 在 项目 跟目录下 bundleconfig.json  中进行压缩，除 base 文件外，其他每个文件对应一个 .min 文件。
 
 
 其他文件说明：除上述文件外：其他文件则是文件上传 存储至本地，可忽略但不建议删除。

  **********************
  以上内容更新于20181112
 **********************