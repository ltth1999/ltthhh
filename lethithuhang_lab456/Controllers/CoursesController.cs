﻿using lethithuhang_lab456.Models;
using lethithuhang_lab456.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace lethithuhang_lab456.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: Courses
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place

            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();
            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var followings = _dbContext.Followings
                .Where(a => a.FolloweeId == userId)
                .Select(a => a.Follower)
                .ToList();

            var viewModel = new FollowingViewModel
            {
                Followings = followings,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
            return View(courses);
        }


  

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Single(c => c.Id == id && c.LecturerId == userId);
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = courses.DateTime.ToString("MM/dd/yyyy"),
                Time = courses.DateTime.ToString("HH:mm"),
                Category = courses.CategoryId,
                Place = courses.Place,
                Heading = "Edit Course",
                Id = courses.Id

            };
            return View("Create", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update (CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Single(c => c.Id == viewModel.Id && c.LecturerId == userId);
            courses.Place = viewModel.Place;
            courses.DateTime = viewModel.GetDateTime();
            courses.CategoryId = viewModel.Category;

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}