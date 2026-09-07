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
                await VerifySecurityAnswersAsync(request, user, cancellationToken);
            }
            var password = request.NewPassword?.Trim();
            if (string.IsNullOrWhiteSpace(password)) {
                throw new PawPalNotFoundException("Password cannot be an empty string");
            }
            var hasher = new PasswordHasher<UserEntity>();
            user.PasswordHash = hasher.HashPassword(null, password);

            // A password change (whether initiated by the user or via the recovery flow)
            // invalidates every existing session: drop all refresh tokens for this user.
            var refreshTokens = await context.RefreshTokens
                .Where(rt => rt.UserId == user.Id)
                .ToListAsync(cancellationToken);
            context.RefreshTokens.RemoveRange(refreshTokens);

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task VerifySecurityAnswersAsync(UpdatePasswordCommand request, UserEntity user, CancellationToken cancellationToken)
        {
            // The set of questions to verify against comes from what is actually
            // registered for this user, never from the question IDs the caller supplies.
            var registeredAnswers = await context.SecurityAnswers
                .Where(x => x.UserId == user.Id)
                .ToDictionaryAsync(x => x.QuestionID, x => x.Answer, cancellationToken);

            var registeredQuestionIds = registeredAnswers.Keys.OrderBy(x => x).ToList();

            if (request.Answers is null ||
                registeredQuestionIds.Count == 0 ||
                !registeredQuestionIds.SequenceEqual(request.Answers.Keys.OrderBy(x => x)))
            {
                throw new PawPalConflictException("Incorrect security answers.");
            }

            var isCorrect = registeredQuestionIds.All(questionId =>
                CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(ConvertToHash(request.Answers[questionId])),
                    Encoding.UTF8.GetBytes(registeredAnswers[questionId])));

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