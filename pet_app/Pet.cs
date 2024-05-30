using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pet_app
{
    internal class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public Pet(string name, int age, string color, string source)
        {
            Name = name;
            Age = age;
            Color = color;
            Image = $@"..\..\..\Images\{source}";
        }

        //adatkötés nélkül a megjelenés vezérlésére jó
        //adatkötéssel ne használjunk ilyet
        //public override string ToString()
        //{
        //    return $"{Name}";
        //}
    }
}
