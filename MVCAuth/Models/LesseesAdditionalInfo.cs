﻿using System;
using System.Collections.Generic;

namespace MVCAuth;

public partial class LesseesAdditionalInfo
{
    public int? Lid { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string Telephone { get; set; } = null!;

    public string PassportId { get; set; } = null!;

    public decimal AvgMark { get; set; }


    public virtual Lessee? LidNavigation { get; set; }
}
