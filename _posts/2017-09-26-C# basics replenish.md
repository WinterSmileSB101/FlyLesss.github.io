---
layout: post
title: c# 基础知识点补充
date: 2017-09-26 09:52:24.000000000 +09:00
---
#### 导语：之前C#很薄弱，熟悉了一些后，总结下C#知识点，知识点不分先后顺序，看到哪总结到哪。
1.全国唯一标识符GUID ：
```
string strguid = Guid.NewGuid().ToString();//57d99d89-caab-482a-a0e9-a0a803eed3ba 生成标准的标志符 (36位标准)
strguid = Guid.NewGuid().ToString("D");//57d99d89-caab-482a-a0e9-a0a803eed3ba 同上，也是标准的标识符 (36位标准)  
strguid = Guid.NewGuid().ToString("N");//38bddf48f43c48588e0d78761eaa1ce6  生成32位无符号标识符  
strguid = Guid.NewGuid().ToString("B");//{09f140d5-af72-44ba-a763-c861304b46f8}  生成(38位:含大括号)  
strguid = Guid.NewGuid().ToString("N");//(778406c2-efff-4262-ab03-70a77d09c2b5)   生成(38位:含小括号)   
```

2.Math.Round 四舍六入五取偶(银行家算法)到指定小数点位数
>[C# Math.Round 文章](http://www.cnblogs.com/lonelyxmas/p/5203494.html)

3.string.IsNullOrEmpty，string.IsNullOrWhiteSpace 
>区别：IsNullOrEmpty: "\t"这样的字符就返回false了,所以要检测这种空白字符用IsNullOrWhiteSpace

4.对象初始化器（必须有一个默认的无参构造函数）
```
  Curry tastcurry = new Curry
        {
            style = "cvf",
            spiciness = 8
        } 

```

5.var关键字（必须初始化，否则无法进行推理）
```
var myVar  = 5;
```
6.匿名类型<br>
7.==,equals区别
> 1、基本数据类型比较
　　==和Equals都比较两个值是否相等。相等为true 否则为false；
　　<br>
2、引用对象比较
　　==和Equals都是比较栈内存中的地址是否相等 。相等为true 否则为false；

8.命名参数(选择使用哪个参数,在方法中有多个可选参数时，在调用的时候选择)
```
  <paramName> ： <paramValue>
  myMethod(value,count:number);
```

9.Lambda表达式
>- 匿名类的简写
>- Lambda 表达式是一个委托

10.LINQ 查询
```
伪代码
　from [type] id in source
　　　　　　[join [type] id in source on expr equals expr [into subGroup]]
　　　　　　[from [type] id in source | let id = expr | where condition]
　　　　　　[orderby ordering,ordering,ordering...]
　　　　　　select expr | group expr by key
　　　　　　[into id query]
```
```
　               from u in users
         　　　　let number = Int32.Parse(u.Username.Substring(u.Username.Length - 1))
         　　　　where u.ID < 9 && number % 2 == 0
         　　　　select u
```
11.类型转换
>- 隐式转换
>- 显示转换(溢出检查 checked  unchecked)
> - Convert命令显示转换
  - as 引用类型转换

12.枚举 enmu 默认是int型
```
enum country : string{
        English = "hello",
        Chinese = "你好"
    }
```
13.参数关键字
> - ref  引用参数，在函数中的改变会改变调用函数的值
>- out   输出参数,使用方式同ref，也是该参数的值将返回给函数调用中的使用变量，使用时，必须把他看成`尚未赋值`，赋值会在执行中丢失
13.可删除的对象
IDisposable 接口 调用的必须实现其Dispose方法， 当不需要某个对象时，就会调用这个方法释放资源，否则要等到垃圾对象回收调用析构方法才会释放资源。`using`关键字可以在代码块初始化使用重要资源对象，在这个代码块结尾，会自动调用Dispose()方法。在方法中调用
```
using (Class1 cls1 = new Class1(), cls2 = new Class1())
            {
                // the code using cls1, cls2
            } //
```
14.多态性：把某个派生类的变量赋值给基本类型的变量。
>比如Animal 是基类 ，有一个 EatFood()方法，派生类是Cow，和Chicken。但是不能以相同点的方法调用派生类自定义的方法。
```
 Cow mycow = new Cow();
    Chicken myChicken = new Chicken();
    Animal myAnimal = mycow;
    myAnimal.EatFood();
```
>接口的多态性 把Animal提供的EatFood放在IConsume接口上，Cow和Chicken也支持这个接口并实现这个方法代码的基础上
```
    Cow mycow = new Cow();
    Chicken myChicken = new Chicken();
    IConsume consumeInterface;
    consumeInterface = myCow;
    consumeInterface.EatFood();
```
15.GetType() 和 typeof 都是获取对象的类型 
> GetType() 是方法，typeof是C#的运算符
16.封箱和拆箱
>封箱是把值类型转换为System.Object类型，或者转换为由值类型实现的接口类型;
 拆箱相反.(值类型是要放在栈上的，而object是引用类型，它需要放在堆上)
 ```
 装箱：
    int age = 24;
    object refAge= age;
 拆箱：
    int  newAge = (int) refAge;
    string newAge =(String) refAge;
 ```
16.可空类型，比如 int 默认为0，可以使他默认为null  `int ? number;`
17.??运算符 
>op1 ?? op2  : 如果第一个操作数不是null，则就等于第一个，否则等于第二个
等同于  op1 == null ? po1 : op2