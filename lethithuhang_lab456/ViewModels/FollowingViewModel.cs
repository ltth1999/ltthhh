using lethithuhang_lab456.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lethithuhang_lab456.ViewModels
{
    public class FollowingViewModel
    {
        public IEnumerable<ApplicationUser> Followings { get; set; }
        public bool ShowAction { get; set; }
    }
}