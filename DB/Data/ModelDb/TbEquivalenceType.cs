using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbEquivalenceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<TbEquivalence> TbEquivalences { get; set; } = new List<TbEquivalence>();
}
