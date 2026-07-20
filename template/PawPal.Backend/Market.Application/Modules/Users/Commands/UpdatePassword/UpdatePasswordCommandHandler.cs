using PawPal.Application.Modules.Users.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.UpdatePassword
{
    public sealed class UpdatePasswordCommandHandler(IAppDbContext context,IAppCurrentUser currentUser,IPasswordHasher<UserEntity> hash) : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);
            if (user == null)
            {
                throw new PawPalNotFoundException($"User with Email {request.Email} does not exist!");
            }
            if (!request.PasswordRecovery)
            {
                if (currentUser.Email != request.Email)
                {
                    throw new PawPalConflictException("User is not allowed to do this action");
                }
                var verify = hash.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
                if (verify == PasswordVerificationResult.Failed)
                    throw new PawPalConflictException("Incorrect password");
            }
            else
            {
                // Anonymous flow: re-verify the security question answers here, server-side,
                // instead of trusting the client's PasswordRecovery flag alone.
                await VerifySecurityAnswersAsync(request, cancellationToken);
            }
            var password = request.NewPassword?.Trim();
            if (string.IsNullOrWhiteSpace(password)) {
                throw new PawPalNotFoundException("Password cannot be an empty string");
            }
            var hasher = new PasswordHasher<UserEntity>();
            user.PasswordHash = hasher.HashPassword(null, password);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task VerifySecurityAnswersAsync(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.Answers is null || request.Answers.Count == 0)
                throw new PawPalConflictException("Security answers are required to reset your password.");

            var questionIds = request.Answers.Keys.ToList();

            var matchingQuestionCount = await context.SecurityQuestions
                .Where(x => questionIds.Contains(x.Id))
                .CountAsync(cancellationToken);
            if (matchingQuestionCount != questionIds.Count)
                throw new PawPalNotFoundException("Question does not exist");

            var storedAnswers = await context.SecurityAnswers
                .Where(x => x.Email == request.Email && questionIds.Contains(x.QuestionID))
                .ToDictionaryAsync(x => x.QuestionID, x => x.Answer, cancellationToken);

            if (storedAnswers.Count != questionIds.Count)
                throw new PawPalConflictException("Incorrect security answers.");

            var providedHashes = request.Answers.OrderBy(x => x.Key).Select(x => ConvertToHash(x.Value)).ToList();
            var storedHashes = storedAnswers.OrderBy(x => x.Key).Select(x => x.Value).ToList();

            var isCorrect = providedHashes.Zip(storedHashes).All(pair =>
                CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(pair.First),
                    Encoding.UTF8.GetBytes(pair.Second)));

            if (!isCorrect)
                throw new PawPalConflictException("Incorrect security answers.");
        }

        private static string ConvertToHash(string answer)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(answer);
            byte[] hashBytes = SHA256.HashData(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }

}