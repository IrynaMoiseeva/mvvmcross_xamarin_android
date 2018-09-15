using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmCross_Application1.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly Lazy<FoodRecyclerViewModel> _foodrecyclerViewModel;
        

        public FoodRecyclerViewModel FoodrecyclerViewModel => _foodrecyclerViewModel.Value;

        

        public MainViewModel(IMvxNavigationService navigationService)
        {
          //  _foodrecyclerViewModel = new Lazy<FoodRecyclerViewModel>(Mvx.IocConstruct<FoodRecyclerViewModel>);
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }
        private IMvxAsyncCommand _navigateCommand;
        private readonly IMvxNavigationService _navigationService;
        
        public async Task SomeMethod(int i)
        {
            MyObject MyObject1 = new MyObject(i);
            await _navigationService.Navigate<FoodRecyclerViewModel, MyObject>(MyObject1);
           
            //Do something with the result MyReturnObject that you get back
        }
        public void ShowFoodList(int y)
        {
            // var presentationBundle = new(new { First = "Hello", Second = "World", Answer = 42 });
          //  ShowViewModel<FoodCartViewModel>();
            //RequestNavigate<FoodRecyclerViewModel>(new { First = "Hello", Second = "World", Answer = 42 }); 
        }

        public void ShowCartList()
        {
            ShowViewModel<FoodCartViewModel>();
        }
    }
}
