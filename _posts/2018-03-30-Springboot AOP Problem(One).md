---
layout: post
title: Springboot AOP Problem(One)
date: 2018-03-30 09:02:24.000000000 +09:00
categories: [problem,AOP]
tag: problem,AOP
---
* 目录
{:toc}
## AOP 运行不进 `@Before`,直接进入`@AfterReturing`
这个问题找了我好久，很奇怪为什么运行时进不去`@Before`,直接进到`@AfterReturing`，后来才发现是我把`@Around`方法里的`pjp.proceed();`注释掉了，导致进不去@Before的方法。具体运行的原理没搞懂...<br>
<br><br>
贴两张在网上找到的顺序图<br>
![AOP1](http://p6b2ow781.bkt.clouddn.com/AOP1.png)<br>
![AOP2](http://p6b2ow781.bkt.clouddn.com/AOP2.png)

