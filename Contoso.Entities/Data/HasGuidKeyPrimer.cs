using Microsoft.EntityFrameworkCore.ChangeTracking;
using Regira.Entities.EFcore.Preppers.Abstractions;
using Regira.Entities.EFcore.Primers.Abstractions;
using Regira.Entities.Models.Abstractions;

namespace Contoso.Data;

internal class HasGuidKeyPrimer : EntityPrimerBase<IEntity<Guid>>
{
    public override Task PrepareAsync(IEntity<Guid> entity, EntityEntry entry)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.CreateVersion7();
        }

        return Task.CompletedTask;
    }
}

internal class HasGuidPrepper : EntityPrepperBase<IEntity<Guid>>
{
    public override Task Prepare(IEntity<Guid> modified, IEntity<Guid>? original)
    {
        if (modified.Id == Guid.Empty)
        {
            modified.Id = Guid.CreateVersion7();
        }

        return Task.CompletedTask;
    }
}