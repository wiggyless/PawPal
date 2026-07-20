namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Delete
{
    public class DeleteAnimalCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) :
        IRequestHandler<DeleteAnimalCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            if (appCurrentUser.RoleId != 3)
                throw new PawPalConflictException("Only administrators can delete animals.");

            var animal = await context.Animals.FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);
            if (animal == null)
                throw new PawPalNotFoundException($"Animal with Id {request.Id} does not exist!");

            animal.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
