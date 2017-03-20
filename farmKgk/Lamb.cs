using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace farmKgk
{
    public class Lamb
    {
        public int id { get; set; }
        public string SerialNumber { get; set; }
        public string Sex { get; set; }
        public string Мonths { get; set; }
        public string Info { get; set; }
        public string Date { get; set; }

        public Lamb()
        {
            //this.SerialNumber = sn;
           // Date = DateTime.Now.ToString("dd MM yyyy");
        }

        public Lamb(string sn, string sex, string months, string info)
        {
            Date = DateTime.Now.ToString("dd MM yyyy");

            this.SerialNumber = sn;
            this.Sex = sex;
            this.Мonths = months;
            this.Info = info;
        }

        public string StringLine()
        {
            if (Мonths.Length == 1)
            {
                if (this.Sex == "Female")
                {
                    return $"{this.Date}          {this.Мonths}          {this.Sex}          {this.SerialNumber}";
                }
                else
                {
                    return $"{this.Date}          {this.Мonths}          {this.Sex}               {this.SerialNumber}";
                }
            }
            else
            {
                if (this.Sex == "Female")
                {
                    return $"{this.Date}          {this.Мonths}        {this.Sex}          {this.SerialNumber}";
                }
                else
                {
                    return $"{this.Date}          {this.Мonths}        {this.Sex}               {this.SerialNumber}";
                }
            }
        }
    }
}
