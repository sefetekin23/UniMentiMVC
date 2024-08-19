using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using UniMenti.Models;

namespace UniMenti.ViewModels
{
    public class RoleVM
    {
        
        
        public string SelectedRole { get; set; }
        public List<AppUser> Users { get; set; }
        public SelectList Roles { get; set; }




    }
}
