using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbEquivalence
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public int IdEquivalenceType { get; set; }

    public DateTime CrerationDate { get; set; }

    public int UserId { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int? UserIdUpdate { get; set; }

    public virtual TbEquivalenceType IdEquivalenceTypeNavigation { get; set; } = null!;
}
