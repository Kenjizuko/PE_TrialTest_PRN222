using System;
using System.Collections.Generic;

namespace Test_Prn222_Trial.Models;

public partial class StoreAccount
{
    public int StoreAccountId { get; set; }

    public string StoreAccountPassword { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string StoreAccountDescription { get; set; } = null!;

    public int? Role { get; set; }
}
