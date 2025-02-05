﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FantasyWebApp.Models;

namespace FantasyWebApp.Controllers
{
    public class PlayersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Players
        //public ActionResult Index()
        //{
        //    return View(db.Players.ToList());
        //}

        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TeamSortParm"] = sortOrder == "Team" ? "team_desc" : "Team";
            ViewData["PosSortParm"] = sortOrder == "Pos" ? "pos_desc" : "Pos";
            ViewData["GradeSortParm"] = sortOrder == "Grade" ? "grade_desc" : "Grade";
            ViewData["CurrentFilter"] = searchString;

            var players = from p in db.Players 
                          select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    players = players.OrderByDescending(p => p.Name);
                    break;
                case "Team":
                    players = players.OrderBy(p => p.Team);
                    break;
                case "date_desc":
                    players = players.OrderByDescending(p => p.Team);
                    break;
                default:
                    players = players.OrderBy(p => p.Grade);
                    break;
            }

            return View(players.AsNoTracking().ToList());
        }


    // GET: Players/ShowSearchForm
    public ActionResult ShowSearchForm()
        {
            return View();
        }

        //Post: Players/ShowSearchResults
        public ActionResult ShowSearchResults(String NameSearchPhrase, String TeamSearchPhrase, String PosSearchPhrase)
        {
            return View("Index", db.Players.Where(p => p.Name.Contains(NameSearchPhrase)).Where(p => p.Team.Contains(TeamSearchPhrase))
                .Where(p => p.Position.Contains(PosSearchPhrase)).ToList());
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Position,Team,Grade,Overview")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Position,Team,Grade,Overview")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
