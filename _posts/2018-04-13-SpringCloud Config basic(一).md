---
layout: post
title: SpringCloud config basic(一)
date: 2018-04-13 10:26:30.000000000 +09:00
categories: [SpringCloud]
tag: SpringCloud
---
* 目录
{:toc}
## **config介绍**
SpringCloudConfig是SpringCloud创建的用来为分布式系统中的基础设施和微服务应用提供集中化的外部配置支持，它分为客户端和服务端两部分。服务端也称为分布式配置中心，是一个独立的微服务应用，用来连接配置仓库并为客户端提供获取配置信息，加密/解密信息等访问接口。而客户端则是微服务架构中各微服务应用或基础设施，通过指定的配置中心来管理应用资源与业务相关的配置内容，并在启动的时候从配置中心获取和加载配置信息。
## **搭建服务端配置**
### 添加pom依赖
```
<dependencies>
  	<dependency>
  		<groupId>org.springframework.cloud</groupId>
  		<artifactId>spring-cloud-config-server</artifactId>
  	</dependency>
  </dependencies>
```
### 添加yml文件配置
```
server:
  port: 3333
spring: 
  application:  
    name: cloud-config
  cloud:
    config: 
      server: 
        git:  
          uri:  https://gitee.com/cocolar/springCloud.git #表示配置中心所在仓库的位置 
        username: #你的git账户
        password: #你的git密码
  
```
### 在启动类中添加注解
```
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.config.server.EnableConfigServer;

@SpringBootApplication
@EnableConfigServer
public class CloudConfigApplication {
	public static void main(String[] args) {
		SpringApplication.run(CloudConfigApplication.class, args);
	}
}

```
### 运行
运行项目，看是否报错。如果遇到指向maven仓库包的问题，可以删了重新update一下，可以解决。运行成功往下看
## **码云仓库**
### 创建文件
在上面配置文件中 我自己在码云上创建了仓库，并分别上传了两个不同环境的配置文件。
文件内容
```
info:
  profile: dev #开发环境  default 为默认环境
```
![http://p6b2ow781.bkt.clouddn.com/config1.png](http://p6b2ow781.bkt.clouddn.com/config1.png)
### 映射关系
```
/{application}/{profile}[/{label}]
/{application}-{profile}.yml或/{application}-{profile}.properties
/{label}/{application}-{profile}.yml或/{label}/{application}-{profile}.properties
这里的url会映射{application}-{profile}.yml对应的配置文件。{label}对应git上不同的分支，默认是master。
```
### 浏览器访问
#### 访问开发环境 [http://localhost:3333/config-client/dev](http://localhost:3333/config-client/dev)
```

	{
	"name": "config-client",
	"profiles": ["dev"],
	"label": null,
	"version": "1ca8d82b97e5d7b1bbc5c481d3961a388eb69bc3",
	"state": null,
	"propertySources": [{
		"name": "https://gitee.com/cocolar/springCloud.git/config-client-dev.yml",
		"source": {
			"info.profile": "dev"
		}
	}, {
		"name": "https://gitee.com/cocolar/springCloud.git/config-client.yml",
		"source": {
			"info.profile": "default"
		}
	}]
}
```
#### 访问默认环境 [http://localhost:3333/config-client.yml](http://localhost:3333/config-client.yml)
```
info:
  profile: default
```
## **本地存储**
配置服务器在从git中获取到配置信息后，实际上会存储一份在config-server的文件系统，也就是复制一份在本地存储。 在控制台会看到添加一份到本地
```
Adding property source: file:/C:/Users/AMBERS~1.LI/AppData/Local/Temp/config-repo-18105772
00124652408/config-client.yml
```
当你断掉网络时，发现获取信息是正常显示的，但是控制台会输出无法连接远端git
```
 Could not fetch remote for master remote: https://gitee.com/cocolar/springCloud.git
```
显示的信息就是从本地获取的。

## **搭建客户端配置**
新建一个SpringBoot项目，命名cloud-config-client
### 引入依赖
```
        <dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-starter-config</artifactId>
		</dependency>
```
### 添加bootstrap.yml 被配置，不是application.yml文件
这些属性必须配置在bootstrap.properties中，config部分内容才能被正确加载。因为config的相关配置会先于application.properties，而bootstrap.yml的加载也是先于application.yml<br>
```
server:
  port: 4444
spring: 
  application:  
    name: config-client  #application的名字
  cloud:
    config: 
      profile:  dev
      uri: http://localhost:3333/   # 服务配置段的地址
```
### 启动类不做改变
```
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class CloudConfigClientApplication {
	public static void main(String[] args) {
		SpringApplication.run(CloudConfigClientApplication.class, args);
	}
}
```
### 创建controller层访问
```
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.cloud.context.config.annotation.RefreshScope;
import org.springframework.core.env.Environment;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RefreshScope
public class InfoController {
  @Value("${info.profile}")
  private String info;
  
  @GetMapping("/info")
  public String info() {
	return this.info;  
  }
  
  //另一种方法
  @Autowired
  private Environment environment;
  @GetMapping("/getinfo")
  public String name_env(){
      return environment.getProperty("info.profile","undefine");
  }
}
```
### 浏览器访问[http://localhost:4444/getinfo](http://localhost:4444/getinfo)
显示  dev
### 客户端从配置管理中获取配置流程：

>应用启动，根据bootstrap.properties中配置的应用名{application}、环境名{profile}、分支名{label}，行配置中心获取配置信息。
>ConfigServer根据自己维护的Git仓库信息和客户端传递过来的配置定位信息去查找配置信息。
>通过git clone命令将找到的配置信息下载到ConfigServer的文件系统中。
>ConfigServer创建Spring的ApplicationContext实例，并从git本地仓库中加载配置文件，最后将这些配置文件内容读取出来返回给客户端
>客户端应用在获得外部配置文件后加载到客户端ApplicationContext实例，该配置内容的优先级高于客户端Jar包内部的配置内容，所以在Jar包中重复的内容将不再被加载。

### 架构
![http://p6b2ow781.bkt.clouddn.com/config2.png](http://p6b2ow781.bkt.clouddn.com/config2.png)
## **配置高可用配置中心**
在微服务架构中，基本每一个服务都会配置成高可用的，配置中心也一样。对上面的config-server进行改造，添加eureka的依赖，是其作为服务在服务注册中心注册。<br>
### 改造配置服务中心
#### 添加依赖
```
<dependencies>
  	<dependency>
  		<groupId>org.springframework.cloud</groupId>
  		<artifactId>spring-cloud-config-server</artifactId>
  	</dependency>
  	<dependency>
  		<groupId>org.springframework.cloud</groupId>
  		<artifactId>spring-cloud-starter-eureka-server</artifactId>
  		<version>1.1.0.RELEASE</version>
  	</dependency>
  </dependencies>
```
### 在启动类上添加`@EnableEurekaClient`注解
开启服务发现功能，使其成为eureka的一个客户端<br>
### 改造配置客户中心
#### 添加依赖
```
<dependencies>
		<dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-starter-config</artifactId>
		</dependency>
		<dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-netflix-eureka-client</artifactId>
		</dependency>
		<dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-netflix-eureka-server</artifactId>
		</dependency>
	</dependencies>
```
#### 在启动类上添加`@EnableDiscoveryClient`注解
#### 在bootstrap.yml添加配置
```
server:
  port: 4444
spring: 
  application:  
    name: config-client
  cloud:
    config: 
      profile:  dev
      uri: http://localhost:3333/
      discovery:                                  #
        enabled:  true                            #开启通过服务访问配置服务中心
        serviceId: cloud-config                   #指定注册的配置服务的名字
eureka:
  client:
    serviceUrl:
      defaultZone: http://localhost:1111/eureka/  # 指定服务注册中心
```
### 运行服务
发现两个服务已经注册<br>
![http://p6b2ow781.bkt.clouddn.com/config3.png](http://p6b2ow781.bkt.clouddn.com/config3.png)<br>
运行之前的controller层，正常显示值dev