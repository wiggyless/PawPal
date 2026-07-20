using PawPal.Application.Abstractions;

namespace PawPal.Application.Modules.Catalog.ProductCategories.Commands.Status.Enable;

public sealed class EnableProductCategoryCommandHandler(IAppDbContext ctx, IAppCurrentUser currentUser)
    : IRequestHandler<EnableProductCategoryCommand, Unit>
{
    public async Task<Unit> Handle(EnableProductCategoryCommand request, CancellationToken ct)
    {
        if (currentUser.RoleId != 3)
            throw new PawPalConflictException("Only administrators can enable categories.");

        var entity = await ctx.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new PawPalNotFoundException($"Category (ID={request.Id}) not found.");

        if (!entity.IsEnabled)
        {
            entity.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        // If already enabled — nothing changes, idempotent
        return Unit.Value;
    }
}