using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiDI003
{
    public interface IDataBase

    {
        public string DataBaseType { get; }

        void Print();
    }

    public class SqlDataBase : IDataBase
    {
        public SqlDataBase(string dataBaseType)
        {
            DataBaseType = dataBaseType;
        }

        public string DataBaseType { get; }

        public void Print()
        {
            Console.WriteLine($"这是一个{DataBaseType }");
        }
    }
}
