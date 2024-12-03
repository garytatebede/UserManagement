namespace UserManagementWebApi.Services;

internal sealed class GuidService : IGuidService
{
    public Guid New() => Guid.NewGuid();
}