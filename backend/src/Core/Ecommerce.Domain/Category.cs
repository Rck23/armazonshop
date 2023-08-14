using Ecommerce.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Domain;

public class Category: BaseDomainModel
{
    [Column(TypeName = "NVARCHAR(100)")]
    public string? Nombre { get; set; }

    public virtual ICollection<Product>? Products { get; set; }
}
