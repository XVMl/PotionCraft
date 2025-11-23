using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionCraft.Content.Items
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class Material:Attribute
    {
        public string Name { get; set; }
        public Material(string name)
        { 
            Name = name;
        }
    }
}
