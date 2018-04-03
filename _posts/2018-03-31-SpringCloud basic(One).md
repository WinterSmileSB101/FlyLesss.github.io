---
layout: post
title: SpringCloud basic(One)
date: 2018-03-31 20:24:30.000000000 +09:00
categories: SpringCloud
tag: SpringCloud
---
## **SpringCloud介绍**
Spring Cloud是一个基于Spring Boot实现的云应用开发工具，它为基于JVM的云应用开发中的配置管理、服务发现、断路器、智能路由、微代理、控制总线、全局锁、决策竞选、分布式会话和集群状态管理等操作提供了一种简单的开发方式。<br>
Spring Cloud包含了多个子项目（针对分布式系统中涉及的多个不同开源产品），比如：Spring Cloud Config、Spring Cloud Netflix、Spring Cloud CloudFoundry、Spring Cloud AWS、Spring Cloud Security、Spring Cloud Commons、Spring Cloud Zookeeper、Spring Cloud CLI等项目。
## **微服务**
微服务架构(比如：netflix、dubbo)就是将一个完整的应用从数据存储开始垂直拆分成多个不同的服务，每个服务都能独立部署、独立维护、独立扩展，服务与服务间通过诸如RESTful API的方式互相调用。
## **SpringCloud的基础搭建**
### 服务注册
这里我们会用到Spring Cloud Netflix，该项目是Spring Cloud的子项目之一，主要内容是对Netflix公司一系列开源产品的包装，它为Spring Boot应用提供了自配置的Netflix OSS整合。通过一些简单的注解，开发者就可以快速的在应用中配置一下常用模块并构建庞大的分布式系统。它主要提供的模块包括：服务发现（Eureka），断路器（Hystrix），智能路有（Zuul），客户端负载均衡（Ribbon）等。<br>
我们这里的核心内容就是服务发现模块：Eureka。<br>
>首先正常创建项目，添加依赖包，勾选如图所示<br>
![create](http://p6b2ow781.bkt.clouddn.com/springcloud1.png)<br>

>pom.xml文件中可以看到添加了下面的依赖<br>

```
<dependencies>
		<dependency>
			<groupId>org.springframework.boot</groupId>
			<artifactId>spring-boot-starter-web</artifactId>
		</dependency>
		<dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-starter-netflix-eureka-server</artifactId>
		</dependency>

		<dependency>
			<groupId>org.springframework.boot</groupId>
			<artifactId>spring-boot-starter-test</artifactId>
			<scope>test</scope>
		</dependency>
	</dependencies>
```
>通过`@EnableEurekaServer`注解启动一个服务注册中心提供给其他应用进行注册<br>

```
package com.example.demo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.netflix.eureka.server.EnableEurekaServer;
//
@EnableEurekaServer
//
@SpringBootApplication
public class TestRegApplication {

	public static void main(String[] args) {
		SpringApplication.run(TestRegApplication.class, args);
	}
}

```

>写配置文件<br>

在默认设置下，该服务注册中心也会将自己作为客户端来尝试注册它自己，所以我们需要禁用它的客户端注册行为，在`application.yml`进行配置。设置端口为1111<br>
```
server:
  port: 1111
  
eureka:
  client:
    register-with-eureka: false
    fetch-registry: false
    serviceUrl:
      defaultZone: http://localhost:${server.port}/eureka/
```

运行项目，在浏览器中访问：http://localhost:1111/ 可以看到并没有客户端注册<br>
![run](http://p6b2ow781.bkt.clouddn.com/springCloud2.png)
### 创建服务提供者
现在我们来创建提供服务的客户端，并且在服务中心注册自己。我们创建一个提供计算a+b的服务模块<br>
>重新创建工程,加入的依赖如下<br>

![](http://p6b2ow781.bkt.clouddn.com/springCloud3.png)
pom.xml中主要依赖  
```
<dependencies>
		<dependency>
			<groupId>org.springframework.boot</groupId>
			<artifactId>spring-boot-starter-web</artifactId>
		</dependency>
		<dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-starter-netflix-eureka-client</artifactId>
		</dependency>
		<dependency>
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-starter-netflix-eureka-server</artifactId>
		</dependency>

		<dependency>
			<groupId>org.springframework.boot</groupId>
			<artifactId>spring-boot-starter-test</artifactId>
			<scope>test</scope>
		</dependency>
	</dependencies>
```
>在主类中通过加上`@EnableDiscoveryClient`注解，该注解能激活Eureka中的DiscoveryClient实现，才能实现Controller中对服务信息的输出。<br>

```
package com.example.demo;

import org.springframework.boot.SpringApplication;

import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
\\
@EnableDiscoveryClient
\\
@SpringBootApplication
public class TestServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(TestServiceApplication.class, args);
	}
}

```
>写配置文件<br>

`spring.application.name`属性，我们可以指定微服务的名称后续在调用的时候只需要使用该名称就可以进行服务的访问。<br>
`eureka.client.serviceUrl.defaultZone`属性对应服务注册中心的配置内容，指定服务注册中心的位置。<br>
注意`spring.application.name`属性的名称最好与启动类`TestServiceApplication.java`名称一致，否则可能出现错误
```
spring:
  application:
    name: test-service
server:
  port: 2223
eureka:
  client:
    serviceUrl:
      defaultZone: http://localhost:1111/eureka/
```
>写controller接口,实现/add请求处理接口，通过DiscoveryClient对象，在日志中打印出服务实例的相关内容。<br>

```
package com.example.demo;

import org.jboss.logging.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class AddController {
	private final Logger logger = Logger.getLogger(getClass());

    @Autowired
    private DiscoveryClient client;

    @RequestMapping(value = "/add" ,method = RequestMethod.GET)
    public Integer add(@RequestParam Integer a, @RequestParam Integer b) {
    	 Integer r = a + b;
    	client.getServices().forEach(id -> {
    		client.getInstances(id).forEach(instance -> {
                logger.info("/add, host:" + instance.getHost() + ", service_id:" + instance.getServiceId()+" result:" + r);
            });
        });
   
        return r;
    }
}

```

>先启动注册中心的服务，在启动该程序运行 http://localhost:1111/，可以看到有注册者了<br>

![run](http://p6b2ow781.bkt.clouddn.com/springCloud4.png)<br>

>我们还可以尝试访问服务提供者提供的接口，发现访问成功。返回正确结果，在控制台中可以看到我们打印的日志<br>

![result](http://p6b2ow781.bkt.clouddn.com/SpringCloud5.png)<br>
![result](http://p6b2ow781.bkt.clouddn.com/SpringCloud6.png)<br>

>ps:有的童鞋不知道运行多个程序，怎么切换console控制台，看图<br>

![console](http://p6b2ow781.bkt.clouddn.com/blog/console.png)<br>
停止服务，旁边的两个叉叉可以清空加载信息哦~

>问题：如果操作一遍发现有些解决不了的问题，可能是包冲突，版本不匹配引起的，建议统一版本<br>

## 源码
注册中心：( https://github.com/FlyLesss/blogCode/tree/master/test-reg )<br>
服务提供者: ( https://github.com/FlyLesss/blogCode/tree/master/test-service )