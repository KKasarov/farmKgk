using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace farmKgk
{
    public class Sheep
    {
        public int id { get; set; }
        public string SerialNumber { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string Info { get; set; }
        public string Date { get; set; }

        public Sheep()
        {
        }

        public Sheep(string sn, string sex, string age, string info)
        {
            Date = DateTime.Now.ToString("dd MM yyyy");

            this.SerialNumber = sn;
            this.Sex = sex;
            this.Age = age;
            this.Info = info;
        }

        public string StringLine()
        {
            if (Age.Length == 1)
            {
                if (this.Sex == "Female")
                {
                    return $"{this.Date}          {this.Age}          {this.Sex}          {this.SerialNumber}";
                }
                else
                {
                    return $"{this.Date}          {this.Age}          {this.Sex}               {this.SerialNumber}";
                }
            }
            else
            {
                if (this.Sex == "Female")
                {
                    return $"{this.Date}          {this.Age}        {this.Sex}          {this.SerialNumber}";
                }
                else
                {
                    return $"{this.Date}          {this.Age}        {this.Sex}               {this.SerialNumber}";
                }
            }          
        }
    }
}
