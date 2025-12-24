using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Create
{
    public sealed class CreateUsersCommandHandler(IAppDbContext context)
        : IRequestHandler<CreateUserCommand, int>
    {
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userName = request.FirstName?.Trim();
            var userLastName = request.LastName?.Trim();
            var userEmail = request.Email?.Trim();
            var birthDate = request.BirthDate;
            var password = request.Password?.Trim();
            var image = request.ProfilePictureURL;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(userLastName) ||
                string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password) || birthDate == null)
            {
                throw new ValidationException("All fields must be filled");
            }
            // checking if the email is being used
            bool emailUsed = await context.Users.
                AnyAsync(x => x.Email == userEmail, cancellationToken);
            if (emailUsed)
            {
                throw new PawPalConflictException("Email is already being used");
            }
            var hasher = new PasswordHasher<UserEntity>();
            var newUser = new UserEntity
            {
                FirstName = userName,
                LastName = userLastName,
                Email = userEmail,
                BirthDate = birthDate,
                ProfilePictureURL = image,
                RoleId = request.RoleID,
            };
            context.Users.Add(newUser);
            await context.SaveChangesAsync(cancellationToken);
            return newUser.Id;
        }
    }
}