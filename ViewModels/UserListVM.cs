using UniMenti.Models;

namespace UniMenti.ViewModels
{
    // UserListViewModel.cs
    public class UserListVM
    {
        public List<AppUser> Users { get; set; }
        public string SearchString { get; set; }
    }

}
