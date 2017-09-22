---
layout: post
title: C# delegate and event 
date: 2017-08-17 09:28:24.000000000 +09:00
---
小白认真的看了看网上写的博客，发现真心好，帮我理解什么叫委托<br>
[参考文章](http://www.tracefact.net/CSharp-Programming/Delegates-and-Events-in-CSharp.aspx)<br>
我看了这篇文章2遍，反复看，才会越来越理解，写一下自己的理解，方便以后查阅，借鉴文章中的热水器的例子<br>
委托(delegate)：委托可以被视为一个更高级的指针，它不仅仅能把地址指向另一个函数，而且还能传递参数，返回值等多个信息。

### Example
```
public void EnglishGreeting(string name) {
    Console.WriteLine("Morning, " + name);
}

public void ChineseGreeting(string name){
    Console.WriteLine("早上好, " + name);
}

public enum Language{
    English, Chinese
}

public void GreetPeople(string name, Language lang){
    //做某些额外的事情，比如初始化之类，此处略
    swith(lang){
        case Language.English:
           EnglishGreeting(name);
           break;
       case Language.Chinese:
           ChineseGreeting(name);
           break;
    }
}
```
&emsp;这个样子如果我还添加其他国家的代码，我还要去修改 GreetPeople这个方法，我想要的是不管添加多少个国家，我的总业务代码是不会变的，也就是GreetPeople这个方法不会变,那么委托就可以帮我们办到。
```
namespace Delegate {
     //定义委托，它定义了可以代表的方法的类型
     public delegate void GreetingDelegate(string name);
        class Program {

           private static void EnglishGreeting(string name) {
               Console.WriteLine("Morning, " + name);
           }

           private static void ChineseGreeting(string name) {
               Console.WriteLine("早上好, " + name);
           }

           //注意此方法，它接受一个GreetingDelegate类型的方法作为参数
           private static void GreetPeople(string name, GreetingDelegate MakeGreeting) {
               MakeGreeting(name);
            }

           static void Main(string[] args) {
             GreetingDelegate delegate1;
            delegate1 = EnglishGreeting; // 先给委托类型的变量赋值
            delegate1 += ChineseGreeting;   // 给此委托变量再绑定一个方法

            // 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            GreetPeople("Jimmy Zhang", delegate1);  
            Console.ReadKey();
           }
        }
    }

输出如下：
Morning, Jimmy Zhang
早上好, 张子阳
```
&emsp;只需要在主函数中绑定事件，GreetPeople代码中添加引用即可，注意这时候的
` public delegate void GreetingDelegate(string name);`是在类的外部，客户端可以随意修改它，接下来就是事件的工作了，封装他。。。,想具体了解，请看参考文档<br>
### 委托、事件与Observer设计模式：
&emsp;假设我们有个高档的热水器，我们给它通上电，当水温超过95度的时候：1、扬声器会开始发出语音，告诉你水的温度；2、液晶屏也会改变水温的显示，来提示水已经快烧开了。

&emsp;现在我们需要写个程序来模拟这个烧水的过程，我们将定义一个类来代表热水器，我们管它叫：Heater，它有代表水温的字段，叫做temperature；当然，还有必不可少的给水加热方法BoilWater()，一个发出语音警报的方法MakeAlert()，一个显示水温的方法，ShowMsg()。
```

 class Heater
    {
        private int temperature;
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;

                if (this.temperature > 95)
                {
                    this.MakeAlert(this.temperature);
                    this.ShowMsg(this.temperature);
                }
            }
            Console.WriteLine("加热");
        }
        public void MakeAlert(int param)
        {
            Console.WriteLine("报警,水已经{0}度了",param);
        }
        public void ShowMsg(int param)
        {
            Console.WriteLine("显示水温,当前温度：{0}度",param);
        }

    }

```
```
 static void Main(string[] args)
        {
            Heater调用
            Heater ht = new Heater();
            ht.BoilWater();
            Console.ReadKey();
        }
```
&emsp;这是最普通的实现办法，让热水器去通知通知温度计和报警器。
&emsp;这时候事件就来啦，事件就是有订阅者和通知者，这里订阅者就相当于温度计和警报器，他们关注了热水器的温度，当热水器的温度有变化时，通知订阅者变化了，自动的去调用。事件就是把委托封装了一层让其变为私有的。
&emsp;这时候应该让温度计和报警器单独成一个类，作为一个独立的个体。
```
 /// <summary>
    /// step2:观察者模式
    /// Subject(隶属，受限制于)被监视的对象，包含其他对象所感兴趣的内容，比如       temperature
    ///Oberver:监视者，监视Subject的行为，当Subject中发生某件事时告诉Observer，并采取相应的行动 
    ///思路： Observer应告诉Subject我对你感兴趣，进行关注，也就是注册；Subject知道后保留对Observer的引用；通过引用自动调用Observer的方法;
    /// </summary>
    /// 热水器
    class Heater1
    {
        private int temperature;
        public delegate void BoilHandler(int param);  //声明委托
        public event BoilHandler BoilEvent; //声明事件
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;

                if (this.temperature > 95)
                {
                    //this.MakeAlert(this.temperature);
                    //this.ShowMsg(this.temperature);
                    if (BoilEvent != null)
                        BoilEvent(temperature);
                }
            }
            Console.WriteLine("加热");
        }
       
      

    }
    /// <summary>
    /// 报警器
    /// </summary>
    class Alarm {
        public void MakeAlert(int param)
        {
            Console.WriteLine("报警");
            Console.WriteLine("报警,水已经{0}度了", param);
        }
    }
    /// <summary>
    /// 显示器
    /// </summary>
    class Display {
        public  static void ShowMsg(int param)
        {
            Console.WriteLine("显示水温,当前温度：{0}度", param);
        }
    }

```
```
static void Main(string[] args)
        {
            //Heater1调用
            Heater1 heater1 = new Heater1();
           
            heater1.BoilEvent += (new Alarm()).MakeAlert; //匿名注册方法
            heater1.BoilEvent += Display.ShowMsg;  //注册静态对象方法

            heater1.BoilWater();   //会自动调用注册过的方法
            Console.ReadKey();
        }
```
以上是最基本的事件和委托的关系。

### .Net Framework中的委托与事件
> .Net Framework的编码规范：<br>
1.委托类型的名称：都应该以EventHandler结束。<br>
2.委托的原型定义：有一个void返回值，并接受两个输入参数：一个Object 类型，一个 EventArgs类型(或继承自EventArgs)。<br>
3.事件的命名为：委托去掉 EventHandler之后剩余的部分。<br>
4.继承自EventArgs的类型应该以EventArgs结尾。<br>

再做一下说明(copy)：

&emsp;委托声明原型中的`Object类型的参数代表了Subject，也就是监视对象`，在本例中是 Heater(热水器)。回调函数(比如Alarm的MakeAlert)可以通过它访问触发事件的对象(Heater)。
`EventArgs 对象包含了Observer所感兴趣的数据，在本例中是temperature`。
```
/// <summary>
    /// 符合 .Net Framework
    /// </summary>
    class Heater2
    {
        private int temperature;
        public string type = "RealFire 001";       // 添加型号作为演示
        public string area = "China Xian";         // 添加产地作为演示
        //public delegate void BoilHandler(int param);  //声明委托
        //public event BoilHandler BoilEvent; //声明事件
        //声明委托
        public delegate void BoiledEventHandler(Object sender, BoiledEventArgs e);
        //声明事件
        public event BoiledEventHandler Boiled;

        // 定义BoiledEventArgs类，传递给Observer所感兴趣的信息
        public class BoiledEventArgs : EventArgs {
            public readonly int temperature;
            public BoiledEventArgs(int temperature) {
                this.temperature = temperature;
            }
        }
        //在基类（父类）中用virtual修饰符声明一个虚方法，然后在在派生类（子类）中用//override修饰符覆盖基类虚方法。表明是对基类的虚方法重载。
        // 可以供继承自 Heater 的类重写，以便继承类拒绝其他对象对它的监视
        protected virtual void OnBoiled(BoiledEventArgs e) {
            if (Boiled != null) {
                Boiled(this,e); //调用所有注册对象的方法
            }
        }
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;

                if (this.temperature > 95)
                {
                    //this.MakeAlert(this.temperature);
                    //this.ShowMsg(this.temperature);

                    //if (BoilEvent != null)
                    //    BoilEvent(temperature);

                    //建立BoiledEventArgs 对象。
                    BoiledEventArgs e = new BoiledEventArgs(temperature);
                    OnBoiled(e);  // 调用 OnBolied方法
                }
            }
            Console.WriteLine("加热");
        }



    }
    /// <summary>
    /// 报警器
    /// </summary>
    class Alarm2
    {
        public void MakeAlert(Object sender,Heater2.BoiledEventArgs e)
        {
            Heater2 heater = (Heater2)sender;

            Console.WriteLine("Alarm：{0} - {1}: ", heater.area, heater.type);
            Console.WriteLine("报警,水已经{0}度了", e.temperature);
        }
    }
    /// <summary>
    /// 显示器
    /// </summary>
    class Display2
    {
        public static void ShowMsg(int param)
        {
            Console.WriteLine("显示水温,当前温度：{0}度", param);
        }
    }

```
```
 static void Main() {
           Heater2 heater = new Heater2();
           Alarm alarm = new Alarm();

           heater.Boiled += alarm.MakeAlert;   //注册方法
           heater.Boiled += (new Alarm()).MakeAlert;      //给匿名对象注册方法
           heater.Boiled += new Heater.BoiledEventHandler(alarm.MakeAlert);    //也可以这么注册
           heater.Boiled += Display.ShowMsg;       //注册静态方法

           heater.BoilWater();   //烧水，会自动调用注册过对象的方法
       }
```
### 总的来说（我的理解）
`委托`相当于是一个类型，同于string ，不同的是，他的变量值是另一个方法的名称，而且定义这个委托时，注意与委托给他的方法参数一致。。。<br>
`事件`就是委托的封装，让他private，但是持有的参数是和委托一致的。
