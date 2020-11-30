using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Interfaces.Services.Identity
{
    public interface IUsersClient :
        IUserRoleStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User>,
        IUserClaimStore<User>,
        IUserLoginStore<User>
    {
    }
}
