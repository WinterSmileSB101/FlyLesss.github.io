---
layout: post
title: SpringCloud Eureka(二)
date: 2018-04-16 14:24:30.000000000 +09:00
categories: [SpringCloud]
tag: SpringCloud
---
## **服务消费者**
在上一遍文章中，我们构建了注册中心和服务提供者，有服务，就要有消费服务的关系，这次我们搭建基于Ribbon的服务消费者。
## **Ribbon**
Ribbon是一个基于HTTP和TCP客户端的负载均衡器。<br>
Ribbon可以在通过客户端中配置的ribbonServerList服务端列表去轮询访问以达到均衡负载的作用。<br>
当Ribbon与Eureka联合使用时，ribbonServerList会被DiscoveryEnabledNIWSServerList重写，扩展成从Eureka注册中心中获取服务端列表。同时它也会用NIWSDiscoveryPing来取代IPing，它将职责委托给Eureka来确定服务端是否已经启动。<br>
## **新建项目**
新建一个项目*test-consumer*<br>
### 添加依赖pom.xml
```
<properties>
		<project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
		<project.reporting.outputEncoding>UTF-8</project.reporting.outputEncoding>
		<java.version>1.8</java.version>
		<spring-cloud.version>Finchley.M9</spring-cloud.version>
	</properties>

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
			<groupId>org.springframework.cloud</groupId>
			<artifactId>spring-cloud-starter-netflix-ribbon</artifactId>
		</dependency>

		<dependency>
			<groupId>org.springframework.boot</groupId>
			<artifactId>spring-boot-starter-test</artifactId>
			<scope>test</scope>
		</dependency>
	</dependencies>
```
### 主类中添加注解
在应用主类中，通过@EnableDiscoveryClient注解来添加发现服务能力。创建RestTemplate实例，并通过@LoadBalanced注解开启均衡负载能力。<br>
```
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
import org.springframework.cloud.client.loadbalancer.LoadBalanced;
import org.springframework.context.annotation.Bean;
import org.springframework.web.client.RestTemplate;

@EnableDiscoveryClient
@SpringBootApplication
public class TestConsumerApplication {

	public static void main(String[] args) {
		SpringApplication.run(TestConsumerApplication.class, args);
	}
	
	@Bean
	@LoadBalanced
	RestTemplate restTemplate() {
		return new RestTemplate();
	}
}
```
### 创建Controller层，来消费service层的服务
```
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

@RestController
public class HelloController {
	@Autowired
    RestTemplate restTemplate;

    @RequestMapping(value = "/hello", method = RequestMethod.GET)
    public String add() {
        return restTemplate.getForEntity("http://test-service/hello", String.class).getBody();
    }

}

```
### 创建配置文件
```
spring:
  application:
    name: test-consumer   #消费者的程序名称
server:
  port: 2222              #本程序的端口
eureka:
  client:
    serviceUrl:
      defaultZone: http://localhost:1111/eureka/   #注册中心的地址
```
### 启动程序
先启动 注册中心 *test-reg*<br>
再启动 服务提供者 *test-service*<br>
在建一个服务提供者  其他都一样，改下端口配置<br>
最后启动 服务消费者 *test-consumer*<br>
输入[http://localhost:1111/](http://localhost:1111/) 查看两个服务已经注册<br>
输入消费者的地址[http://localhost:2222/hello](http://localhost:2222/hello)输出hello<br>
这样就实现了负载均衡，Ribbon会轮询访问服务实例，关掉其中一个实例，会自动去寻找下一个。
## **源码**
我这里只建了一个服务实例。
注册中心：[https://github.com/FlyLesss/blogCode/tree/master/test-reg](https://github.com/FlyLesss/blogCode/tree/master/test-reg)<br>
服务提供者: [https://github.com/FlyLesss/blogCode/tree/master/test-service](https://github.com/FlyLesss/blogCode/tree/master/test-service)
服务消费者：[https://github.com/FlyLesss/blogCode/tree/master/test-consumer](https://github.com/FlyLesss/blogCode/tree/master/test-consumer)