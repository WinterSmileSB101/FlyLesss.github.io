---
layout: post
title: SpringCloud rabbitMQ 整合Bus 
date: 2018-04-13 14:04:30.000000000 +09:00
categories: [SpringCloud]
tag: SpringCloud
---
## **前言**
当git上内容更新时，要对每个服务实例去发送Post请求，通知更新。而使用Bus(消息总线)对一个服务实例进行更新时,将刷新请求发送到消息总线上,其他在消息总线上的服务实例就会获取到，重新再config-server获取配置文件,从而实现配置信息的动态更新。<br>
![http://p6b2ow781.bkt.clouddn.com/bus1.png](http://p6b2ow781.bkt.clouddn.com/bus1.png)<br>
请求流程
![http://p6b2ow781.bkt.clouddn.com/bus2.png](http://p6b2ow781.bkt.clouddn.com/bus2.png)<br>
## 遇到的问题
我建的多模块maven工程，添加version就是会报错，添加了很多个版本都有问题
```
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-actuator</artifactId>
    <version>2.0.1.RELEASE</version>
</dependency>
```
报的错
```
Parameter 0 of method traceFilterRegistration in org.springframework.cloud.netflix.eureka.server.EurekaServerAutoConfiguration required a bean of type 'javax.servlet.Filter' that could not be found.
	- Bean method 'resourceUrlEncodingFilter' in 'FreeMarkerServletWebConfiguration' not loaded because @ConditionalOnEnabledResourceChain did not find class org.webjars.WebJarAssetLocator
	- Bean method 'remoteDevToolsDispatcherFilter' not loaded because @ConditionalOnProperty (spring.devtools.remote.secret) did not find property 'secret'
```
后来我把version去掉，并且放到主依赖中，自动下的事2.0.0版本,运行正常....
不晓得什么原因，应该就是包冲突，坑...
## spring cloud 集成项目
Spring Cloud Task：提供云端计划任务管理、任务调度。<br>
Spring Cloud Connectors：便于云端应用程序在各种PaaS平台连接到后端，如：数据库和消息代理服务。
