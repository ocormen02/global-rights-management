
namespace GlobalRightsManagement.Core.Models
{
    public sealed record MusicContract(
        string Artist,
        string Title,
        string[] Usages,
        DateOnly StartDate,
        DateOnly? EndDate
    );
}
