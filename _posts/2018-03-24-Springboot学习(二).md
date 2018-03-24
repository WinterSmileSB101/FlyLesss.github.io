---
layout: post
title: Springboot Learn(二)--CURD
date: 2018-03-24 20:16:24.000000000 +09:00
categories: Spring boot 
tag: Spring boot
---
## **前言**
上一篇文章已经介绍过用Springboot搭建HelloWorld,今天来用Springboot进行对Mysql数据库的增删改查操作基于RestFul规范。怎么创建项目见上一篇文章<br>
## **创建数据库**
```
CREATE TABLE `ec_partner` (
  `PARTNER_ID` varchar(32) NOT NULL DEFAULT '' COMMENT '合作伙伴编号',
  `USER_TYPE` varchar(2) NOT NULL COMMENT '用户类型(1个人、2企业、3商户、4店小二)',
  `LOGIN_ACCOUNT` text NOT NULL COMMENT '登录账号',
  `PASSWORD` text NOT NULL COMMENT '密码',
  `NAME` text COMMENT '姓名',
  `MOBILE` text COMMENT '手机号码',
  `EMAIL` text COMMENT '邮箱地址',
  `IDCARD` text COMMENT '证件号码',
  `COMPANY_NAME` text COMMENT '公司名称',
  `COMPANY_ADDRESS` text COMMENT '公司地址',
  PRIMARY KEY (`PARTNER_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='合作伙伴信息';


--添加数据
INSERT INTO `ec_partner` VALUES ('11', '11', '11@11', '11', '11', '11', '11', '11', '11', '11');
INSERT INTO `ec_partner` VALUES ('15', '15', '15', '15', '15', '15', '15', '15', '15', '15');
INSERT INTO `ec_partner` VALUES ('16', '16', '16', '16', '16', '16', '16', '16', '16', '16');
INSERT INTO `ec_partner` VALUES ('33', '33', '33', '33', '33', '33', '33', '33', '33', '33');
INSERT INTO `ec_partner` VALUES ('44', '44', '44', '44', '44', '44', '44', '44', '44', '44');
INSERT INTO `ec_partner` VALUES ('55', '56', '56', '56', '56', '56', '56', '56', '56', '56');
INSERT INTO `ec_partner` VALUES ('66', '66', '66', '66', '66', '66', '66', '66', '66', '66');
INSERT INTO `ec_partner` VALUES ('678', '3', '678', '678', '678', '678', '678', '678', '678', '678');
INSERT INTO `ec_partner` VALUES ('77', '77', '77', '77', '77', '77', '77', '77', '77', '77');
INSERT INTO `ec_partner` VALUES ('88', '88', '88', '88', '88', '88', '88', '88', '88', '88');
INSERT INTO `ec_partner` VALUES ('99', '99', '99', '99', '99', '99', '99', '99', '99', '99');

```
## **配置application.properties文件**
yml文件对格式规范很有要求，多敲一个空格很可能造成错误，最好复制本段代码<br>
我的数据库名字叫`user`
```
spring:
  datasource:
    url: jdbc:mysql://localhost:3306/user?characterEncoding=UTF-8&useSSL=false
    username: root
    password: 123456
    driver-class-name: com.mysql.jdbc.Driver
```

## **搭建基础工程**
把包的结构创建好，基于MVC模式<br>
`bean`：放实体类，包括get和set方法<br>
`mapper`: 放dao层和xml文件<br>
`service`: 业务处理层<br>
`web`: 控制层<br>
`config`: 放配置的类<br>
`util`: 工具层<br>
编写代码的顺序是 bean --> mapper  -->service  -->web<br>
![curd1](/images/curd1.png)

## **编写代码**
### 首先是bean层
```
package com.example.demo.bean;

public class Partner {

	private String partnerId;

	private String userType;

	private String loginAccount;

	private String password;

	private String name;

	private String mobile;

	private String email;

	private String idcard;

	private String companyName;

	private String companyAddress;

	public String getPartnerId() {
		return partnerId;
	}

	public void setPartnerId(String partnerId) {
		this.partnerId = partnerId;
	}

	public String getUserType() {
		return userType;
	}

	public void setUserType(String userType) {
		this.userType = userType;
	}

	public String getLoginAccount() {
		return loginAccount;
	}

	public void setLoginAccount(String loginAccount) {
		this.loginAccount = loginAccount;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getMobile() {
		return mobile;
	}

	public void setMobile(String mobile) {
		this.mobile = mobile;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getIdcard() {
		return idcard;
	}

	public void setIdcard(String idcard) {
		this.idcard = idcard;
	}

	public String getCompanyName() {
		return companyName;
	}

	public void setCompanyName(String companyName) {
		this.companyName = companyName;
	}

	public String getCompanyAddress() {
		return companyAddress;
	}

	public void setCompanyAddress(String companyAddress) {
		this.companyAddress = companyAddress;
	}

	@Override
	public String toString() {
		return "User [partnerId=" + partnerId + ", userType=" + userType + ", loginAccount=" + loginAccount
				+ ", password=" + password + ", name=" + name + ", mobile=" + mobile + ", email=" + email + ", idcard="
				+ idcard + ", companyName=" + companyName + ", companyAddress=" + companyAddress + "]";
	}
}

```
### mapper层
两个class文件，一个`.java` 另一个`.xml`,两个文件的文件名要`一样`<br>
这里会用到一个mybatis的注解`@Mapper`<br>
PartnerMapper.java<br>
```
ackage com.example.demo.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.example.demo.bean.Partner;


@Mapper
public interface PartnerMapper {
	
	//注意下命名规范
	List<Partner> findPartner();
	Partner findPartnerById( String partnerId);
	int insertPartner(Partner user);
	int updatePartner(Partner user);
	int deletePartnerById(String partnerId);
}

```
PartnerMapper.xml<br>
这里介绍下`resultMap`：resultMap是Mybatis最强大的元素，它可以将查询到的复杂数据（比如查询到几个表中数据）映射到一个结果集当中。<br>
```
<!-resultMap包含的元素：-->
<!--column不做限制，可以为任意表的字段，而property须为type 定义的pojo属性-->
<resultMap id="唯一的标识" type="映射的pojo对象">
  <id column="表的主键字段，或者可以为查询语句中的别名字段" jdbcType="字段类型" property="映射pojo对象的主键属性" />
  <result column="表的一个字段（可以为任意表的一个字段）" jdbcType="字段类型" property="映射到pojo对象的一个属性（须为type定义的pojo对象中的一个属性）"/>
  <association property="pojo的一个对象属性" javaType="pojo关联的pojo对象">
    <id column="关联pojo对象对应表的主键字段" jdbcType="字段类型" property="关联pojo对象的主席属性"/>
    <result  column="任意表的字段" jdbcType="字段类型" property="关联pojo对象的属性"/>
  </association>
  <!-- 集合中的property须为oftype定义的pojo对象的属性-->
  <collection property="pojo的集合属性" ofType="集合中的pojo对象">
    <id column="集合中pojo对象对应的表的主键字段" jdbcType="字段类型" property="集合中pojo对象的主键属性" />
    <result column="可以为任意表的字段" jdbcType="字段类型" property="集合中的pojo对象的属性" />  
  </collection>
</resultMap>
```
注意下面#{*****}应为bean里对应的属性名，而不是表名
```
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE mapper PUBLIC "-//mybatis.org//DTD Mapper 3.0//EN" "http://mybatis.org/dtd/mybatis-3-mapper.dtd">
<mapper namespace="com.example.demo.mapper.PartnerMapper">
	<resultMap id="userResultMap" type="com.example.demo.bean.Partner">
		<result column="PARTNER_ID" property="partnerId"></result>
		<result column="USER_TYPE" property="userType"></result>
		<result column="LOGIN_ACCOUNT" property="loginAccount"></result>
		<result column="PASSWORD" property="password"></result>
		<result column="NAME" property="name"></result>
		<result column="MOBILE" property="mobile"></result>
		<result column="EMAIL" property="email"></result>
		<result column="IDCARD" property="idcard"></result>
		<result column="COMPANY_NAME" property="companyName"></result>
		<result column="COMPANY_ADDRESS" property="companyAddress"></result>
	</resultMap>
	<select id="findPartner" resultMap="userResultMap"
		parameterType="java.lang.String">
		select * from ec_partner
	</select>
	<select id="findPartnerById" resultMap="userResultMap"
		parameterType="java.lang.String">
		select * from ec_partner
		where PARTNER_ID=#{partnerId}
	</select>
	<insert id="insertPartner" parameterType="com.example.demo.bean.Partner">
		insert into ec_partner(
		PARTNER_ID,
		USER_TYPE,
		LOGIN_ACCOUNT,
		PASSWORD,
		NAME,
		MOBILE,
		EMAIL,
		IDCARD,
		COMPANY_NAME,
		COMPANY_ADDRESS
		)values(
		#{partnerId},
		#{userType},
		#{loginAccount},
		#{password},
		#{name},
		#{mobile},
		#{email},
		#{idcard},
		#{companyName},
		#{companyAddress}
		)
	</insert>
	<delete id="deletePartnerById" parameterType="java.lang.String">
		delete from ec_partner
		where PARTNER_ID=#{partnerId}
	</delete>
</mapper>
```
### service层
#### service.api
```
package com.example.demo.service.api;

import java.util.List;

import com.example.demo.bean.Partner;

public interface IPartnerService {
	
	List<Partner> findPartner();
	Partner findPartnerById( String partnerId);
	int insertPartner(Partner user);
	int updatePartner(Partner user);
	int deletePartnerById(String partnerId);
}

```

#### service.impl
这里会用到两个注解`@Service`和`@Autowired`
```
package com.example.demo.service.impl;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.demo.bean.Partner;
import com.example.demo.mapper.PartnerMapper;
import com.example.demo.service.api.IPartnerService;

@Service
public class PartnerService implements IPartnerService {

	@Autowired
	PartnerMapper partnerMapper;
	@Override
	public List<Partner> findPartner() {
		// TODO Auto-generated method stub
		return partnerMapper.findPartner();
	}

	@Override
	public Partner findPartnerById(String partnerId) {
		// TODO Auto-generated method stub
		return partnerMapper.findPartnerById(partnerId);
	}

	@Override
	public int insertPartner(Partner user) {
		// TODO Auto-generated method stub
		return partnerMapper.insertPartner(user);
	}

	@Override
	public int updatePartner(Partner user) {
		// TODO Auto-generated method stub
		return partnerMapper.updatePartner(user);
	}

	@Override
	public int deletePartnerById(String partnerId) {
		// TODO Auto-generated method stub
		return partnerMapper.deletePartnerById(partnerId);
	}

}

```
### web层
这里用到两个注解`@RestController`和`@RequestMapping("***")`
参数两个注解 `@RequestParam`和`@RequestBody`
```
package com.example.demo.web;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.bean.Partner;
import com.example.demo.service.impl.PartnerService;

//RestController：默认类中的方法都会以json的格式返回。
@RestController
@RequestMapping("/partner")
public class PartnerController {
	
	@Autowired
	PartnerService partnerService;
	
	@RequestMapping(value="/findPartner",method={RequestMethod.GET})
	public List<Partner> findPartner() {
		// TODO Auto-generated method stub
		return partnerService.findPartner();
	}

	@RequestMapping(value="/findPartnerById",method={RequestMethod.GET})
	public Partner findPartnerById(@RequestParam(value = "partnerId", required = true)String partnerId) {
		// TODO Auto-generated method stub
		return partnerService.findPartnerById(partnerId);
	}

	@RequestMapping(value="/insertPartner",method={RequestMethod.POST})
	public int insertPartner(@RequestBody Partner user) {
		// TODO Auto-generated method stub
		return partnerService.insertPartner(user);
	}

	@RequestMapping(value="/updatePartner",method={RequestMethod.PUT})
	public int updatePartner(@RequestBody Partner user) {
		// TODO Auto-generated method stub
		return partnerService.updatePartner(user);
	}

	@RequestMapping(value="/deletePartnerById",method={RequestMethod.DELETE})
	public int deletePartnerById(@RequestParam(value = "partnerId", required = true) String partnerId) {
		// TODO Auto-generated method stub
		return partnerService.deletePartnerById(partnerId);
	}


}


```
## **测试结果**
获取全部信息<br>
![success2](/images/success2.png)
获取单个信息<br>
![success3](/images/success3.png)
更新，增加...
可以用postman工具测，我这里就不演示了，下篇文章会演示swagger，一个界面测试的插件
