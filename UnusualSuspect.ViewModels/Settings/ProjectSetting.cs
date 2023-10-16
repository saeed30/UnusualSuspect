namespace UnusualSuspect.ViewModels.Settings;

public class ProjectSetting
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public JwtSettings JwtSettings { get; set; }
    public IdentitySettings IdentitySettings { get; set; }
    public SiteSetting SiteSetting { get; set; }
    public FireBaseSetting FireBaseSetting { get; set; }
    public AdminUser AdminUser { get; set; }
  
}
