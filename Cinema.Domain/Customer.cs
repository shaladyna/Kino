using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cinema.Domain
{
    internal class Customer
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        public Customer(string name, string email, string phone)
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidCustomerDataException("Name cannot be empty.");

            if (!email.Contains("@") || !email.Split('@').Last().Contains("."))
                throw new InvalidCustomerDataException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(phone) || phone.Length < 9 || !phone.All(c => char.IsDigit(c) || c == '+'))
                throw new InvalidCustomerDataException("Phone must have at least 9 digits and contain only digits or '+'.");

            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}