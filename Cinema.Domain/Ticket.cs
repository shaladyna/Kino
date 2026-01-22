using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Cinema.Domain
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(NormalTicket), "normal")]
    [JsonDerivedType(typeof(StudentTicket), "student")]
    [JsonDerivedType(typeof(VipTicket), "vip")]
    public abstract class Ticket
    {
        public string Name { get; protected set; }

        protected Ticket(string name)
        {
            Name = name;
        }

        public abstract decimal GetPrice(decimal basePrice);

        public virtual string GetDisplayName()
        {
            return $"Ticket: {Name}";
        }
    }

    public class NormalTicket : Ticket
    {
        public NormalTicket() : base("Normal") { }
        public override decimal GetPrice(decimal basePrice) => basePrice;
    }

    public class StudentTicket : Ticket
    {
        public StudentTicket() : base("Student") { }
        public override decimal GetPrice(decimal basePrice) => basePrice * 0.8m;
    }

    public class VipTicket : Ticket
    {
        public VipTicket() : base("VIP") { }
        public override decimal GetPrice(decimal basePrice) => basePrice * 1.5m;

        public override string GetDisplayName()
        {
            return $"*** {Name.ToUpper()} CLASS TICKET ***";
        }
    }
}