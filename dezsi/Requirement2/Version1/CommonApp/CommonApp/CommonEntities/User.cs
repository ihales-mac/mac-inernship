using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp.Model
{

    public class User
    {



        string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password != value)
                {
                    _Password = value;


                }
            }
        }

        string _Username;

        public User(string Password, string Username)
        {
            _Password = Password;
            _Username = Username;
        }

        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (_Username != value)
                {
                    _Username = value;


                }
            }
        }

    }




}
