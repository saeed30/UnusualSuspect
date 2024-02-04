
namespace UnusualSuspect.ViewModels.Settings;

public class ProjectSetting
{
	public bool IsTesting { get; set; }
	public RateLimiterSetting RateLimiterSetting { get; set; }
	public ConnectionStrings ConnectionStrings { get; set; }
	public JwtSettings JwtSettings { get; set; }
	public IdentitySettings IdentitySettings { get; set; }
	public SiteSetting SiteSetting { get; set; }
	public HangfireSetting HangfireSetting { get; set; }
	public FireBaseSetting FireBaseSetting { get; set; }
	public AdminUser AdminUser { get; set; }
	public GameSetting GameSetting { get; set; }

}
