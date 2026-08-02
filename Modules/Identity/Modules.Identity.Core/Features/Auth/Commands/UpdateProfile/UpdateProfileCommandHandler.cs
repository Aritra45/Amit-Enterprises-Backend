using MediatR;
using Modules.Identity.Core.Abstractions;
using Shared.Core.Abstractions;
using Shared.Core.Exceptions;
using Shared.Core.Wrapper;

namespace Modules.Identity.Core.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, IResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public UpdateProfileCommandHandler(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<IResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null)
        {
            throw new UnauthorizedException("User is not authenticated.");
        }

        var user = await _userRepository.GetByIdAsync(_currentUser.UserId.Value, cancellationToken)
            ?? throw new NotFoundException("User", _currentUser.UserId.Value);

        user.FullName = request.FullName;
        user.MobileNumber = request.MobileNumber;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success("Profile updated successfully.");
    }
}
