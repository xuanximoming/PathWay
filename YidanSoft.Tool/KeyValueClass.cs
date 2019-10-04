using System;
using System.Collections.Generic;
namespace DrectSoft.Tool
{
    public class KeyValue
    {
        public KeyValue()
        {


        }
        public KeyValue(String key, String value)
        {
            Key = key;
            Value = value;

        }
        public KeyValue(String key, String value, String name)
        {
            Key = key;
            Value = value;
            Name = name;
        }
        private String _Key = "";
        public String Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        private String _Value = "";
        public String Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private String _Name = "";

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

    }
    public class KeyValues : List<KeyValue>
    {
        public KeyValue this[String key]
        {
            get
            {
                KeyValue KeyValueTemp = null;
                foreach (var item in this)
                {
                    if (item.Key == key)
                    {
                        KeyValueTemp = item;
                    }
                }
                return KeyValueTemp;
            }
        }
    }
}
