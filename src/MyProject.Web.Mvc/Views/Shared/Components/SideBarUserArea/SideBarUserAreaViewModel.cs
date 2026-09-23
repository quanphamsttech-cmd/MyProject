using MyProject.Sessions.Dto;

namespace MyProject.Web.Views.Shared.Components.SideBarUserArea;

public class SideBarUserAreaViewModel
{
    public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

    public bool IsMultiTenancyEnabled { get; set; }

    public string GetShownLoginName()
    {
        return LoginInformations.User.UserName;
    }
}