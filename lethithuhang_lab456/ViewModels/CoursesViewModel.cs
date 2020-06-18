using lethithuhang_lab456.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lethithuhang_lab456.ViewModels
{
    public class CoursesViewModel
    {
        public IEnumerable<Course> UpcommingCourses { get; set; }
        public IEnumerable<Following> Followings { get; set; }
        public IEnumerable<Attendance> Attendances { get; set; }
        public bool ShowAction { get; set; }
    }
}