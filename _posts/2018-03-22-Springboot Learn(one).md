---
layout: post
title: Springboot Learn(一)--HelloWorld
date: 2018-03-22 22:08:24.000000000 +09:00
categories: Spring boot 
tag: Spring boot
---
## _Springboot介绍_<br>
使用Spring Boot可以让我们快速创建一个基于Spring的项目，而让这个Spring项目跑起来我们只需要很少的配置就可以。<br>
## _Springboot功能介绍_<br>
1.独立运行的Spring项目<br>
2.提供starter简化Maven配置<br>
3.Spring Boot可以内嵌Tomcat，这样我们无需以war包的形式部署项目。<br>
4.自动配置Spring <br>
5.我目前理解到的就这些，以后了解了在补充...<br>
## _Springboot安装环境和运行软件_
1.安装JDK<br>
我的jdk安装目录是<br>
```
C:\Program Files\Java\jdk1.8.0_152
```
所以环境变量中配置(一定要查看对应的文件中是否有配置的文件)
```
JAVA_HOME=C:\Program Files\Java\jdk1.8.0_152
path=%JAVA_HOME%\bin
CLASSPATH=%JAVA_HOME%\lib\dt.jar;%JAVA_HOME%\lib\tools.jar
```
环境配好后，安装软件
[下载地址](http://spring.io/tools/sts/)
## _建立第一个HelloWorld_
1.File--->New--->Spring Starter Project<br>
2.![image01](/images/start1.png)<br>都是默认点击Next<br>
3.把需要用到的包勾上，我这里经常使用这几个，就有显示经常使用的包。
![image02](/images/start2.png)
`DevTools`:热启动，就是不用重新加载服务器<br>
`Mybatis`:Mybatis的依赖<br>
`Mysql`:mysql的依赖<br>
`Web`:spring的核心包<br>
`Validation`:校验依赖<br>
点击Finish<br>
这里第一次建，需要些时间，他需要从网上下载依赖包。可以查看右下角的进度条。<br>
4.建好的工程如图<br>
![image03](/images/project1.png)<br>
`DemoTestApplication.java`是程序的启动入口<br>
`application.yml`或者`application.properties`都可以，是配置属性的。<br>
5.`DemoTestApplication.java`这个类有一个`@SpringBootApplication`注解，这是整个Spring Boot的核心注解，它的目的就是开启Spring Boot的自动配置。<br>
```
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.jdbc.DataSourceAutoConfiguration;

@SpringBootApplication(exclude
= {DataSourceAutoConfiguration.class})
public class DemoTestApplication {

	public static void main(String[] args) {
		SpringApplication.run(DemoTestApplication.class, args);
	}
}
```

`exclude= {DataSourceAutoConfiguration.class}`没有这个属性会报错

```
***************************
APPLICATION FAILED TO START
***************************
Description:
Failed to auto-configure a DataSource: 'spring.datasource.url' is not specified and no embedded datasource could be auto-configured.
Reason: Failed to determine a suitable driver class
Action:
Consider the following:
	If you want an embedded database (H2, HSQL or Derby), please put it on the classpath.

	If you have database settings to be loaded from a particular profile you may need to activate it (no profiles are currently active).
```
要排除此类的autoconfig。启动以后就可以正常运行。

这是因为添加了数据库组件，所以autoconfig会去读取数据源配置，而我新建的项目还没有配置数据源，所以会导致异常出现
6.新建一个Controller类，实现一个HelloWorld
```
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
//RestController：默认类中的方法都会以json的格式返回。
@RestController
public class DemoController {
	
	@RequestMapping("/hello")
	String index(){
        return "Hello Spring Boot!";
	}
}
```
7.右键项目运行 Run as --->Spring boot App<br>
8.浏览器测试<br>
![image04](/images/success1.png)<br>
[点击查看源码](https://github.com/FlyLesss/blogCode/tree/master/demoHelloWorld)


