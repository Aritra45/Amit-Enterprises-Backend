using MediatR;
using Shared.Core.Wrapper;

namespace Modules.Master.Core.Features.Settings.Commands.UploadLogo;

public record UploadLogoCommand(Stream FileStream, string FileName) : IRequest<Result<string>>;
