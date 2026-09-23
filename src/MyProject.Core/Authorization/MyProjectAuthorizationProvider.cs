using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace MyProject.Authorization;

public class MyProjectAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        // ==============================
        // USERS
        // ==============================

        context.CreatePermission(
            PermissionNames.Pages_Users,
            L("Users"));

        context.CreatePermission(
            PermissionNames.Pages_Users_Activation,
            L("UsersActivation"));


        // ==============================
        // ROLES
        // ==============================

        context.CreatePermission(
            PermissionNames.Pages_Roles,
            L("Roles"));


        // ==============================
        // TENANTS
        // ==============================

        context.CreatePermission(
            PermissionNames.Pages_Tenants,
            L("Tenants"),
            multiTenancySides: MultiTenancySides.Host);


        // ==============================
        // ADMISSION QUOTA
        // ==============================

        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota,
            L("AdmissionQuota"));

        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota_Create,
            L("AdmissionQuotaCreate"));

        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota_Edit,
            L("AdmissionQuotaEdit"));

        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota_Submit,
            L("AdmissionQuotaSubmit"));

        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota_Approve,
            L("AdmissionQuotaApprove"));

        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota_Reject,
            L("AdmissionQuotaReject"));
        context.CreatePermission(
            PermissionNames.Pages_AdmissionQuota_Delete,
            L("AdmissionQuotaDelete"));
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(
            name,
            MyProjectConsts.LocalizationSourceName);
    }
}