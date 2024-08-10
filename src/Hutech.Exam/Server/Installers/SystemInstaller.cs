
namespace Hutech.Exam.Server.Installers
{
    public class SystemInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
        }
    }
}
