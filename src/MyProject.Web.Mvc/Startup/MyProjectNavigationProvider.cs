using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using MyProject.Authorization;

namespace MyProject.Web.Startup;

/// <summary>
/// This class defines menus for the application.
/// </summary>
public class MyProjectNavigationProvider : NavigationProvider
{
    public override void SetNavigation(INavigationProviderContext context)
    {
        context.Manager.MainMenu

            // TRANG CHỦ
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Home,
                    L("HomePage"),
                    url: "",
                    icon: "fas fa-home",
                    requiresAuthentication: true
                )
            )

            // KỲ TUYỂN SINH
            .AddItem(
                new MenuItemDefinition(
                    "AdmissionPeriods",
                    new FixedLocalizableString("Kỳ tuyển sinh"),
                    url: "AdmissionPeriods",
                    icon: "fas fa-calendar-alt",
                    permissionDependency: new SimplePermissionDependency(
                        PermissionNames.Pages_AdmissionQuota
                    )
                )
            )

            // QUẢN LÝ CHỈ TIÊU
            .AddItem(
                new MenuItemDefinition(
                    "AdmissionQuota",
                    new FixedLocalizableString("Quản lý chỉ tiêu"),
                    url: "AdmissionQuota",
                    icon: "fas fa-graduation-cap",
                    permissionDependency: new SimplePermissionDependency(
                        PermissionNames.Pages_AdmissionQuota
                    )
                )
            )

            // NGƯỜI DÙNG
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Users,
                    L("Users"),
                    url: "Users",
                    icon: "fas fa-users",
                    permissionDependency: new SimplePermissionDependency(
                        PermissionNames.Pages_Users
                    )
                )
            )

            // VAI TRÒ
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Roles,
                    L("Roles"),
                    url: "Roles",
                    icon: "fas fa-theater-masks",
                    permissionDependency: new SimplePermissionDependency(
                        PermissionNames.Pages_Roles
                    )
                )
            );
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, MyProjectConsts.LocalizationSourceName);
    }
}