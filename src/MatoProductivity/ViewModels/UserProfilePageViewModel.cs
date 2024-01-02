using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class UserProfilePageViewModel : ViewModelBase, ISingletonDependency
    {


        public UserProfilePageViewModel(
            )
        {

            //Init();
        }

    }






}
