using MVCAuth;
using System;
using System.Collections.Generic;

namespace MVCAuth;

public partial class Lessee
{
    public int Lid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<FlatsContract> FlatsContracts { get; set; } = new List<FlatsContract>();

    public virtual ICollection<HousesContract> HousesContracts { get; set; } = new List<HousesContract>();

    public virtual ICollection<RoomsContract> RoomsContracts { get; set; } = new List<RoomsContract>();
    public virtual LesseesAdditionalInfo LesseesAdditionalInfo { get; set; } = new LesseesAdditionalInfo();

}
