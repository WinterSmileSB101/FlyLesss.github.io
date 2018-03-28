---
layout: post
title: Springboot Learn(Three)--Swagger
date: 2018-03-28 22:52:24.000000000 +09:00
categories: Spring boot 
tag: Spring boot
---
## **Swagger介绍**
本文将介绍RESTful API的重磅好伙伴Swagger2，它可以轻松的整合到Spring Boot中，并与Spring MVC程序配合组织出强大RESTful API文档。它既可以减少我们创建文档的工作量，同时说明内容又整合入实现代码中，让维护文档和修改代码整合为一体，可以让我们在修改代码逻辑的同时方便的修改文档说明。另外Swagger2也提供了强大的页面测试功能来调试每个RESTful API。
### 作用:

    1. 接口的文档在线自动生成。

    2. 功能测试。
###  Swagger是一组开源项目，其中主要要项目如下：

1.   Swagger-tools:提供各种与Swagger进行集成和交互的工具。例如模式检验、Swagger 1.2文档转换成Swagger 2.0文档等功能。

2.   Swagger-core: 用于Java/Scala的的Swagger实现。与JAX-RS(Jersey、Resteasy、CXF...)、Servlets和Play框架进行集成。

3.   Swagger-js: 用于JavaScript的Swagger实现。

4.   Swagger-node-express: Swagger模块，用于node.js的Express web应用框架。

5.   Swagger-ui：一个无依赖的HTML、JS和CSS集合，可以为Swagger兼容API动态生成优雅文档。

6.   Swagger-codegen：一个模板驱动引擎，通过分析用户Swagger资源声明以各种语言生成客户端代码。
### Swagger使用的注解及其说明：

@Api：用在类上，说明该类的作用。

@ApiOperation：注解来给API增加方法说明。

@ApiImplicitParams : 用在方法上包含一组参数说明。

@ApiImplicitParam：用来注解来给方法入参增加说明。

@ApiResponses：用于表示一组响应

@ApiResponse：用在@ApiResponses中，一般用于表达一个错误的响应信息

    l   code：数字，例如400

    l   message：信息，例如"请求参数没填好"

    l   response：抛出异常的类   

@ApiModel：描述一个Model的信息（一般用在请求参数无法使用@ApiImplicitParam注解进行描述的时候）

    l   @ApiModelProperty：描述一个model的属性

##  **准备工作**
### 添加Swagger依赖
在maven的pom.xml文件中添加
```
 <dependency>
            <groupId>io.springfox</groupId>
            <artifactId>springfox-swagger2</artifactId>
            <version>2.2.2</version>
        </dependency>

        <dependency>
            <groupId>io.springfox</groupId>
            <artifactId>springfox-swagger-ui</artifactId>
            <version>2.2.2</version>
        </dependency>
```
添加后几个maven 中update下，把Force update选上<br>
### 添加Swagger的配置类
放在config包下
```
@Configuration
@EnableSwagger2
public class Swagger2Config {
    private final String swagger2_api_basepackage = "com.example.demo.web";  
    private final String swagger2_api_title = "用户-API";  
    private final String swagger2_api_description = "用户信息的api";  
    private final String swagger2_api_contact = "ls";  
    private final String swagger2_api_version = "1.0";  
    /** 
     * createRestApi 
     * 
     * @return 
     */  
    @Bean  
    public Docket createRestApi() {  
        return new Docket(DocumentationType.SWAGGER_2)  
                .apiInfo(apiInfo())  
                .select()  
                .apis(RequestHandlerSelectors.basePackage(swagger2_api_basepackage))  
                .paths(PathSelectors.any())  
                .build();  
    }  
    /** 
     * apiInfo 
     * @return 
     */  
    private ApiInfo apiInfo() {  
        return new ApiInfoBuilder()  
                .title(swagger2_api_title)  
                .description(swagger2_api_description)  
                .contact(swagger2_api_contact)  
                .version(swagger2_api_version)  
                .build();  
    }  
}

```
`@Configuration`配置注解<br>
`@EnableSwagger2`启用Swagger注解，一定要加上<br>
`swagger2_api_basepackage`写的是controller层的包名，也就是web层，也是最上面的一层，跟前端界面进行接收和返回值得一层。
## **代码的修改**
### bean层实体类Partner就变成了这样
`@ApiModelProperty(value = "partnerId",dataType = "String")`中的value是在界面上显示这个属性的含义，所以可以用中文来标识
```
package com.example.demo.bean;

import io.swagger.annotations.ApiModel;
import io.swagger.annotations.ApiModelProperty;

@ApiModel(value = "Partner的实体，----》",reference = "我是参考")
public class Partner {
	
	@ApiModelProperty(value = "partnerId",dataType = "String")
	private String partnerId;
	@ApiModelProperty(value = "userType",dataType = "String")
	private String userType;
	@ApiModelProperty(value = "loginAccount",dataType = "String")
	private String loginAccount;
	@ApiModelProperty(value = "password",dataType = "String")
	private String password;
	@ApiModelProperty(value = "name",dataType = "String")
	private String name;
	@ApiModelProperty(value = "mobile",dataType = "String")
	private String mobile;
	@ApiModelProperty(value = "email",dataType = "String")
	private String email;
	@ApiModelProperty(value = "idcard",dataType = "String")
	private String idcard;
	@ApiModelProperty(value = "companyName",dataType = "String")
	private String companyName;
	@ApiModelProperty(value = "companyAddress",dataType = "String")
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
### web层的修改
这里增加了API的说明`@ApiOperation`，以及参数的说明`ApiImplicitParam`，可以在界面上进行显示
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

import io.swagger.annotations.ApiImplicitParam;
import io.swagger.annotations.ApiOperation;

//RestController：默认类中的方法都会以json的格式返回。
@RestController
@RequestMapping("/partner")
public class PartnerController {
	
	@Autowired
	PartnerService partnerService;
	
	//查询
    @ApiOperation(value="获取用户列表", notes="获取列表")
	@RequestMapping(value="/findPartner",method={RequestMethod.GET})
	public List<Partner> findPartner() {
		// TODO Auto-generated method stub
		return partnerService.findPartner();
	}
    @ApiOperation(value="获取单个用户信息", notes="获取用户")
	@RequestMapping(value="/findPartnerById",method={RequestMethod.GET})
	public Partner findPartnerById(@RequestParam(value = "partnerId", required = true)String partnerId) {
		// TODO Auto-generated method stub
		return partnerService.findPartnerById(partnerId);
	}

    @ApiOperation(value="增加用户信息", notes="增加用户")
	@RequestMapping(value="/insertPartner",method={RequestMethod.POST})
	public int insertPartner(@RequestBody Partner partner) {
		// TODO Auto-generated method stub
		return partnerService.insertPartner(partner);
	}

    @ApiOperation(value="更新用户信息", notes="更新用户")
    @ApiImplicitParam(name = "partner", value = "partner实体", required = true, dataType = "Partner")
	@RequestMapping(value="/updatePartner",method={RequestMethod.PUT})
	public int updatePartner(@RequestBody Partner partner) {
		// TODO Auto-generated method stub
		return partnerService.updatePartner(partner);
	}
    
    @ApiOperation(value="删除用户信息", notes="删除")
	@RequestMapping(value="/deletePartnerById",method={RequestMethod.DELETE})
	public int deletePartnerById(@RequestParam(value = "partnerId", required = true) String partnerId) {
		// TODO Auto-generated method stub
		return partnerService.deletePartnerById(partnerId);
	}


}

```
### 测试验证
浏览器输入http://localhost:8080/swagger-ui.html<br>
端口默认是8080，没有修改过的<br>
![Swagger_ui](http://p6b2ow781.bkt.clouddn.com/QQ%E5%9B%BE%E7%89%8720180328224200.png)
点击`try it out`!进行对各种API的测试，非常方便<br>
[点击查看源码地址](https://github.com/FlyLesss/blogCode/tree/master/demoSwagger)
