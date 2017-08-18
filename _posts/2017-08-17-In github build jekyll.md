---
layout: post
title: GitHub上利用Jekyll框架搭建个人博客
date: 2017-08-17 09:28:24.000000000 +09:00
---

### 导语：<br>
 自己搭建博客在个github上部署了好久一直是404，后来某一天才发现是自己理解错了，总结下来。
### GitHub Pages
>GitHub Pages 是由官方托管和发布的，你可以使用 GitHub 提供的页面自动生成器。也可以做个人博客，是个轻量级的博客系统，没有麻烦的配置。使用标记语言如Markdown，不需自己搭建服务器，还可以绑定自己的域名。我这里没有绑定域名，所以不作介绍。
### GitHub 域名的创建
> 在Github上创建仓库，但是仓库名字一定要是 xxx.github.io  格式一定要是这个样子，xxx是github注册的用户名，不能是其他，这个是在master分支上规定的，如果想用其他域名，就新创建一个gh-pages,再来自定义xxx的名字。
### 把创建的仓库克隆到本地
>在本地git配置好以及和github远程连接建立好后，进行如下步骤。
1.把刚刚创建的仓库克隆到本地 git clone xxx <br>
2.把[Vno-jekyll](https://github.com/onevcat/vno-jekyll) clone到仓库下，注意clone后vno-jekyll文件夹里面的内容提到仓库根目录下，删除vno-jekyll文件。我之前就是把vno-jekyll直接传上去了，没有比我更蠢的了，就报一直找不到index.html.<br>
3.安装 ruby 百度  安装ruby后才能执行gem操作<br>
4.cmd 下 执行gem install jekyll （vpn快点）<br>
  测试 gem source  是否安装成功<br>
5.然后  git切换到仓库根目录下<br>
  执行bundle install<br>
  如果出现bundle: command not found<br> 
  先安装bundle: gem install bundle<br>
6.一切就绪后<br>
   开启jekyll环境 ：bundle exec jekyll serve<br> 
   ![](/images/run.jpg)
   在浏览器上访问 http://127.0.0.1:4000/<br>
见到喵神就对了<br>
7.把代码都push上去  (以下命令只针对我的电脑)<br>
   git add .<br>
   git cmmmit -m "blog"<br>
   git push origin master<br>
8.在浏览器上访问 xxx.github.io就可以看到了。<br>

### 具体搭建后的具体操作，我也是小白，由于时间关系，后续更新。图片之后再插。<br>
[参考文章](http://www.jianshu.com/p/88c9e72978b4)

