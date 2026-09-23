namespace MyProject.Authorization
{
    public static class PermissionNames
    {
        // ==============================
        // USERS
        // ==============================

        public const string Pages_Users =
            "Pages.Users";

        public const string Pages_Users_Activation =
            "Pages.Users.Activation";


        // ==============================
        // ROLES
        // ==============================

        public const string Pages_Roles =
            "Pages.Roles";


        // ==============================
        // TENANTS
        // ==============================

        public const string Pages_Tenants =
            "Pages.Tenants";


        // ==============================
        // ADMISSION QUOTA
        // ==============================

        // Quyền xem/quản lý chỉ tiêu tuyển sinh
        public const string Pages_AdmissionQuota =
            "Pages.AdmissionQuota";

        // Quyền thêm chỉ tiêu
        public const string Pages_AdmissionQuota_Create =
            "Pages.AdmissionQuota.Create";

        // Quyền sửa chỉ tiêu
        public const string Pages_AdmissionQuota_Edit =
            "Pages.AdmissionQuota.Edit";

        // Quyền gửi chỉ tiêu đi duyệt
        public const string Pages_AdmissionQuota_Submit =
            "Pages.AdmissionQuota.Submit";

        // Quyền duyệt chỉ tiêu
        public const string Pages_AdmissionQuota_Approve =
            "Pages.AdmissionQuota.Approve";

        // Quyền từ chối chỉ tiêu
        public const string Pages_AdmissionQuota_Reject =
            "Pages.AdmissionQuota.Reject";
        public const string Pages_AdmissionQuota_Delete =
            "Pages.AdmissionQuota.Delete";
    }
}