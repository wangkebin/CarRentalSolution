using System.ComponentModel;

namespace CarRental.Domain.Entities;

public enum CarCategory
{
    //[Description("Undefined")]
    Undefined = 0,
     
    //[Description("Sedan")]
    Sedan = 1,

    //[Description("SUV")]
    SUV = 2,

    //[Description("Van")]
    Van = 3
}