---
layout: post
title: SpringCloud problem
date: 2018-04-03 10:05:30.000000000 +09:00
categories: SpringCloud
tag: SpringCloud
---
* 目录
{:toc}
### 搭建Springcloud基础模块并实现3个  服务注册中心     用户    商品,
在用户往商品里传递值时，一直报415传不过去。
```
  String pro =JSON.toJSONString(product);
	    HttpHeaders headers = new HttpHeaders();
	    headers.setContentType(MediaType.APPLICATION_JSON);

	    HttpEntity<String> entity = new HttpEntity<String>(pro,headers);
	    return restTemplate.postForEntity("http://demo-client/product/insertProduct",entity,String.class).getBody();
```
要把Header 的ContentType 变成 UTF-8，要不识别不到，可以观察浏览器F12传过去的类型。随便放一张图片，显示要查看的位置<br>
![problem1](http://p6b2ow781.bkt.clouddn.com/problem1.png)
### RestTemplate在put  delete的时候没有返回值，可以用exchange()方法
```
            JSONObject jo = new JSONObject();
	    	jo.put("comyId", 4);
	    	jo.put("comName", "pppp");
	    	 HttpHeaders headers = new HttpHeaders();
	 	    headers.setContentType(MediaType.APPLICATION_JSON);

	 	    HttpEntity<JSONObject> entity = new HttpEntity<JSONObject>(jo,headers);
	    	return restTemplate.exchange("http://demo-client/product/updateProductByPId",HttpMethod.PUT,entity,Object.class);
```