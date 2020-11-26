using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleTrader.Domain.Exceptions
{
    public class InvalidPasswordExeption : Exception
    {
        public string Username { get; set; }
        public string Password { get; set; }


        public InvalidPasswordExeption(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public InvalidPasswordExeption(string message, string username, string password) : base(message)
        {
            Username = username;
            Password = password;
        }

        public InvalidPasswordExeption(string message, Exception innerException, string username, string password) : base(message, innerException)
        {
            Username = username;
            Password = password;
        }
    }
}
