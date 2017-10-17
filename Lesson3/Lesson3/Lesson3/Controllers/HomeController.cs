﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lesson3.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Sample()
        {
            return View();
        }
        public IActionResult Index2()
        {
            ViewBag.Title = "This is Index2";
            return View();
        }


        public IActionResult Index3()
        {
            ViewBag.Title = "This is Index3";
            Person person = new Person();
            return View(person);
        }
        [HttpGet]
        public IActionResult ValidateAge()
        {
            ViewBag.Title = "Validate Age for voting";
            Person person1 = new Person();
            return View(person1);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

    }
}
