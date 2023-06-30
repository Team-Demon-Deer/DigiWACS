using Microsoft.AspNetCore.Authorization;

namespace DigiWACS.Server.Authorization.Requirements;

public class AdminRoleRequirement : IAuthorizationRequirement {
	public bool IS_ADMIN { get; }
	public AdminRoleRequirement() {
		IS_ADMIN = true;
	}
}
