﻿using System;
using System.Linq;
using System.Web.Mvc;
using Interactive.HateBin.Controllers.Helpers;
using Interactive.HateBin.Data;
using Interactive.HateBin.Models;

namespace Interactive.HateBin.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private UserRepository userRepository => new UserRepository();
        private HateRepository hateRepository => new HateRepository();

        private const int PageSize = 20;

        public ActionResult Index(int id = 0, PageDirection dir = PageDirection.Forward)
        {
            var currentUser = userRepository.GetByEmail(HttpContext.User.Identity.Name);
            Guid? token = currentUser.Token;
            if (token.Value == Guid.Empty) token = null;
            var myhate = hateRepository.GetList(id, PageSize + 1, dir, token).ToList();

            return View(new HateListViewModel
            {
                HateList = myhate.Take(PageSize).OrderByDescending(x => x.Id).ToList(),
                ShowPrev = PagingHelper.ShowPrev(id, myhate, dir, PageSize),
                ShowNext = PagingHelper.ShowNext(myhate, dir, PageSize)
            });
        }
    }
}