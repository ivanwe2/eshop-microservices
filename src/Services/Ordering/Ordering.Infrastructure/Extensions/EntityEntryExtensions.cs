﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Extensions
{
    public static class EntityEntryExtensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry)
            => entry.References.Any(r=> 
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == Microsoft.EntityFrameworkCore.EntityState.Added 
                || r.TargetEntry.State == Microsoft.EntityFrameworkCore.EntityState.Modified));
    }
}
