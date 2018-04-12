---
layout: post
title: SpringCloud RabbitMQ basic(一)
date: 2018-04-12 16:09:30.000000000 +09:00
categories: [SpringCloud]
tag: SpringCloud
---
* 目录
{:toc}
## **AMQP**
在异步通讯中，消息不会立刻到达接收方，而是被存放到一个容器中，当满足一定的条件之后，消息会被容器发送给接收方，这个容器即消息队列，而完成这个功能需要双方和容器以及其中的各个组件遵守统一的约定和规则，AMQP就是这样的一种协议，消息发送与接受的双方遵守这个协议可以实现异步通讯。这个协议约定了消息的格式和工作方式。
## **RabbitMQ**
AMQP 是一种协议， RabbitMQ是一个由erlang开发的AMQP的开源实现，目前使用比较广泛的MQ有RabbitMQ,ActiveMQ,KafKa等等，其中ActiveMQ是基于JMS的一个开源实现，JMS 是一个接口标准或者说是一个API消息服务的规范（JAVA Message Service，java消息服务），KafKa是一种高吞吐量的分布式发布订阅消息系统，通常有吞吐量需求的日志处理和日志聚合应用会使用Kafka，性能要优于Rabbit,但是稳定性和可靠性相对而言RabbitMQ要成熟一些。
### **安装**
下载Erlang：
http://www.erlang.org/downloads<br>
下载rabbitMQ:
http://www.rabbitmq.com/download.html<br>
安装完成后进行配置：<br>
Erlang
>首先需要保证ERLANG_HOME环境配置正确
>        在环境变量中添加  ERLANG_HOME的路劲，如D:\Program Files\erl6.3
>
>        然后在PATH中添加%ERLANG_HOME%\bin
>
>       在cmd中输入erl ，如果能弹出erlang shell界面则表示配置正确了

rabbitMQ

>配置RabbitMQ
>    打开命令行模式cmd：
>    cd C:\rabbitmq\RabbitMQ\Server\rabbitmq_server-3.0.0\sbin
>     依次输入：
>     1. ./rabbitmq-plugins.bat enable rabbitmq_management
>      2. ./rabbitmq-service.bat stop
>      3.rabbitmq-service.bat install   这句话没用，提示：RabbitMQ service is already present - onlyupdating service parameters
>      4. ./rabbitmq-service.bat start
>      重新 start  install   stop  在执行第一个语句就能进去网页。
>      打开浏览器登录：http://127.0.0.1:15672 
>     55672好像也可以，直接跳转到下列位置 
>     http://127.0.0.1:15672/#/
>     账号和密码都是guest

![http://p6b2ow781.bkt.clouddn.com/rabbitmq.png](http://p6b2ow781.bkt.clouddn.com/rabbitmq.png)<br>
Admin选显卡，可以创建一个springcloud账户，tags是rabbitmq的角色分类。<br>
![http://p6b2ow781.bkt.clouddn.com/rabbitmq1.png](http://p6b2ow781.bkt.clouddn.com/rabbitmq1.png)
## **快速搭建基本工程**
### 添加依赖
```
<dependencies>
		<dependency>
				<groupId>org.springframework.boot</groupId>
				<artifactId>spring-boot-starter-amqp</artifactId>
                <version>2.0.1.RELEASE</version>
			</dependency>
	</dependencies>
```
### 创建自己的springcloud账户
在admin选项卡中进行创建<br>
点击创建好的账户，设置virtual hosts ,否则一会链接会出错<br>
![http://p6b2ow781.bkt.clouddn.com/rabbitmq3.png](http://p6b2ow781.bkt.clouddn.com/rabbitmq3.png)
创建好的样子<br>
![http://p6b2ow781.bkt.clouddn.com/rabbitmq2.png](http://p6b2ow781.bkt.clouddn.com/rabbitmq2.png)
### 配置yml文件
端口就是5672，不是15672，这个地方坑了我....
```
server:
  port: 6666
spring: 
  application:  
    name: cloud-rabbitmq
  rabbitmq:
    host: localhost
    port: 5672  #
    username: springcloud
    password: springcloud
```
### 配置消息生产者
```
import java.util.Date;
import org.springframework.amqp.core.AmqpTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;


@Component
public class Sender {

	@Autowired 
	private AmqpTemplate rabbitTemplate;
	
    public void send() {
    	String context = "hello" +new Date();
    	System.out.println("sender:"+context);
    	this.rabbitTemplate.convertAndSend("hello",context);
    }
}

```
### 配置消息消费者
```
import org.springframework.amqp.rabbit.annotation.RabbitHandler;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

@Component
@RabbitListener(queues="hello")
public class Receiver {

	@RabbitHandler
	public void process(String hello) {
		System.out.println("receiver:"+hello);
	}
}
```
### 配置基本配置类 
```
import org.springframework.amqp.core.Queue;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitConfig {

	@Bean
	public Queue helloQueue() {
		return new Queue("hello");
	}
}

```

### 创建测试类
```
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Future;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;

import com.example.demo.util.Sender;
@RunWith(SpringRunner.class)
@SpringBootTest
public class RabbitmqApplicationTests {

	@Autowired
	private Sender sender;
	@Test
	public void contextLoads() {
	}
    
	@Test
	public void hello() throws Exception{
		sender.send();
	}
     
}

```
### 项目结构图
![http://p6b2ow781.bkt.clouddn.com/rabbitmq4.png](http://p6b2ow781.bkt.clouddn.com/rabbitmq4.png)<br>
### 启动项目
看到RabbitMQ Connections和Channels 连接条目<br>
![http://p6b2ow781.bkt.clouddn.com/rabbitmq5.png](http://p6b2ow781.bkt.clouddn.com/rabbitmq5.png)<br>
在启动测试类 <br>
控制台输出：sender:helloThu Apr 12 15:40:51 CST 2018<br>
切换到主控制台输出：<br>
receiver:helloThu Apr 12 15:40:51 CST 2018
