using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbElement
{
    public int Id { get; set; }

    public int IdElementType { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public int? IdElementFather { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreationUser { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int? UpdateUser { get; set; }

    public bool? Enable { get; set; }

    public virtual TbElement? IdElementFatherNavigation { get; set; }

    public virtual ICollection<TbElement> InverseIdElementFatherNavigation { get; set; } = new List<TbElement>();

    public virtual ICollection<TbLogElement> TbLogElements { get; set; } = new List<TbLogElement>();
}
