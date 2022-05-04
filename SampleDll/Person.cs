using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDll
{
    public class Calc
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
        public int Subtract(int x, int y)
        {
            return x - y;
        }
        public int Multiply(int x, int y)
        {
            return x * y;
        }
        public int DivideBy(int x, int y)
        {
            return x / y;
        }
        public double ShowPI()
        {
            return 3.14;
        }
        public override string ToString()
        {
            return "A Calc";
        }
    }
    public class Person
    {
        private string sampleField = string.Empty;
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int ID { get; set; }
        public Person()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            ID = 0;
        }
        public Person(int id,string firstName,string lastName):this(firstName,lastName)
        {
          
            ID = id;
           
        }
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public override string ToString()
        {
            return "A Person";
        }
        public string ShowData()
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendFormat("ID: {0}, FirstName: {1}, LastName: {2}", ID, FirstName, LastName);
            return sb.ToString();
        }
       
    }
}
