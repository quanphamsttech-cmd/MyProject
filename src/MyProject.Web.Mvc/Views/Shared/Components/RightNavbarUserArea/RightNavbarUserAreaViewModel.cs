using MyProject.Sessions.Dto;

namespace MyProject.Web.Views.Shared.Components.RightNavbarUserArea;

public class RightNavbarUserAreaViewModel
{
    public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

    public bool IsMultiTenancyEnabled { get; set; }

    public string GetShownLoginName()
    {
        var userName = LoginInformations.User.UserName;

        return userName;
    }
}