using MyProject.Roles.Dto;
using System.Collections.Generic;

namespace MyProject.Web.Models.Common;

public interface IPermissionsEditViewModel
{
    List<FlatPermissionDto> Permissions { get; set; }
}