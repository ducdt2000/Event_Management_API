using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EventAttribute : Attribute
    {
        public string PropertyName { get; set; }
        public string Messenger { get; set; }
        protected EventAttribute(string propertyName, string messenger = "")
        {
            PropertyName = propertyName;
            Messenger = messenger;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Required : EventAttribute
    {
        public Required(string propertyName) : base(propertyName, $"{propertyName} không được phép để trống. ")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Duplicate : EventAttribute
    {
        public Duplicate(string propertyName) : base(propertyName, $"Trùng {propertyName}. ")
        {
        }
    }

    public class MaxLength : EventAttribute
    {
        public int LengthMax;

        public MaxLength(string propertyName, int length = int.MaxValue) : base(propertyName, $"Độ dài tối đa của {propertyName} là {length}. ")
        {
            this.LengthMax = length;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MinLength : EventAttribute
    {
        public int LengthMin;
        public MinLength(string propertyName, int length = int.MinValue) : base(propertyName, $"Độ dài tối thiểu của {propertyName} là {length}. ")
        {
            this.LengthMin = length;
        }
    }
}
