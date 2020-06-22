using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI002
{
    public interface IMyService
    {
        void Print(string method);
    }

    public class MyService : IMyService
    {
        public void Print(string method)
        {
            Console.WriteLine(DateTime.Now);
        }
    }
    public class MyService1 : IMyService
    {
        int printCount;
        public void Print(string method)
        {
            printCount++;
            Console.WriteLine($"--打印次数{printCount}，method:{method},当前日期：{DateTime.UtcNow}");
        }
    }

    public interface IYouService
    {
        void Print(string method);
    }

    public class YouService : IYouService
    {
        private IMyService _myService;
        public YouService(IMyService myService)
        {
            _myService = myService;
        }
        public void Print(string method)
        {
            _myService.Print(method);
        }
    }
}
