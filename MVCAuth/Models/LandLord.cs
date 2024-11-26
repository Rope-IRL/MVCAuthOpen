using MVCAuth;
using System;
using System.Collections.Generic;


namespace MVCAuth;
public partial class LandLord
{
    public int Llid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Flat>? Flats { get; set; } = new List<Flat>();

    public virtual ICollection<FlatsContract>? FlatsContracts { get; set; } = new List<FlatsContract>();

    public virtual ICollection<Hotel>? Hotels { get; set; } = new List<Hotel>();

    public virtual ICollection<House>? Houses { get; set; } = new List<House>();

    public virtual ICollection<HousesContract>? HousesContracts { get; set; } = new List<HousesContract>();

    public virtual LandLordsAdditionalInfo? LandLordsAdditionalInfo { get; set; } = new LandLordsAdditionalInfo();

    public virtual ICollection<RoomsContract>? RoomsContracts { get; set; } = new List<RoomsContract>();
}
