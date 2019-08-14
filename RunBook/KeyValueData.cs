using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBook
{
    public class KeyValueData : ViewModelBase
    {

        public KeyValueData()
        {                    
        }

        public KeyValueData(string k, string v)
        {
            Key = k;
            Value = v;
        }
        private string _key;
        private string _value;

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (value != _key)
                {
                    _key = value;
                    OnPropertyChanged("Key");
                }

            }

        }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
    }
}
