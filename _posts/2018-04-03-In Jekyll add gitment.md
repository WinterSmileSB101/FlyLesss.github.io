---
layout: post
title: In Jekyll add gitment(添加评论系统)
date: 2018-04-03 20:28:30.000000000 +09:00
categories: [jekyll,gitment]
tag: jekyll gitment
---
* 目录
{:toc}
### 在之前搭建的Jekyll上添加评论系统（gitment）
网上很多第三方系统，不是被墙就是被关闭，所以借鉴一篇博客搭建国内大神写的gitment系统<br>
[https://blog.csdn.net/ggabcda/article/details/77141221](https://blog.csdn.net/ggabcda/article/details/77141221)
这是国内大神的项目主页
[github.com/imsun/gitment ](github.com/imsun/gitment)
### 申请一个Github OAuth Application
Github头像下拉菜单 > Settings > 左边Developer settings下的OAuth Application > Register a new application，填写相关信息：

Application name, Homepage URL, Application description 都可以随意填写
Authorization callback URL 一定要写自己Github Pages的URL
填写完上述信息后按Register application按钮，得到Client ID和Client Secret
![comments](http://p6b2ow781.bkt.clouddn.com/comment1.png)
###  在jekyll博客调用gitment
如gitment项目页Readme所示，在你需要添加评论系统的地方，一般是_layout/目录下的 post.html, 添加一下代码
>
>```
><div id="gitmentContainer"></div>
><link rel="stylesheet" >href="https://imsun.github.io/gitment/style/default.css">
><script >src="https://imsun.github.io/gitment/dist/gitment.browser.>js"></script>
><script>
>var gitment = new Gitment({
>   owner: 'Your GitHub username',
>    repo: 'The repo to store comments',
>    oauth: {
>        client_id: 'Your client ID',
>        client_secret: 'Your client secret',
>    },
>});
>gitment.render('gitmentContainer');
></script>
```
需要修改的有4个地方

Your GitHub username：填写你的Github Pages博客所在的github账户名
The repo to store comments：填写用来存放评论的github仓库，由于评论是 通过issues来存放的，个人建议这里可以直接填Github Pages个人博客所在的仓库
Your client ID：第1步所申请到的应用的Client ID
Your client secret：第1步所申请到的应用的Client Secret
填写完这4项把代码保存上传到github就可以了。
![http://p6b2ow781.bkt.clouddn.com/comment2.png](http://p6b2ow781.bkt.clouddn.com/comment2.png)
### 为每篇博文初始化评论系统
由于gitment的原理是为每一遍博文以其URL作为标识创建一个github issue， 对该篇博客的评论就是对这个issue的评论。因此，我们需要为每篇博文初始化一下评论系统， 初始化后，你可以在你的github上会创建相对应的issue。

接下来，介绍一下如何初始化评论系统

上面第2步代码添加成功并上传后，你就可以在你的博文页下面看到一个评论框，还 有看到以下错误Error: Comments Not Initialized，提示该篇博文的评论系统还没初始化

点击Login with GitHub后，使用自己的github账号登录后，就可以在上面错误信息 处看到一个Initialize Comments的按钮
(ps: 由于要求回调URL和当前地址一样，故第2步不能在本地调试， 需把代码先上传再调试)

点击Initialize Comments按钮后，就可以开始对该篇博文开始评论了， 同时也可以在对应的github仓库看到相应的issue
![http://p6b2ow781.bkt.clouddn.com/comment3.png](http://p6b2ow781.bkt.clouddn.com/comment3.png)
###  我有一页的博客初始化失败，暂时还没找到原因...
