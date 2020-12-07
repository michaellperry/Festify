using System;
using System.IO;
using System.Threading.Tasks;
using Festify.Promotion.Contents;
using Festify.Promotion.Shows;
using Microsoft.AspNetCore.Mvc;

namespace Festify.Promotion.Acts
{
    public class ActsController : Controller
    {
        private readonly ActCommands actCommands;
        private readonly ActQueries actQueries;
        private readonly ContentCommands contentCommands;
        private readonly ShowQueries showQueries;

        public ActsController(ActCommands actCommands, ActQueries actQueries, ContentCommands contentCommands, ShowQueries showQueries)
        {
            this.actCommands = actCommands;
            this.actQueries = actQueries;
            this.contentCommands = contentCommands;
            this.showQueries = showQueries;
        }

        // GET: Acts
        public async Task<IActionResult> Index()
        {
            return View(await actQueries.ListActs());
        }

        // GET: Acts/Details/abc-123
        public async Task<IActionResult> Details(Guid id)
        {
            var act = await actQueries.GetAct(id);
            if (act == null)
            {
                return NotFound();
            }

            var shows = await showQueries.ListShows(id);
            var viewModel = new ActViewModel
            {
                Act = act,
                Shows = shows
            };

            return View(viewModel);
        }

        // GET: Acts/Create
        // GET: Acts/Create/abc-123
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Create), new { id = Guid.NewGuid() });
            }
            return View();
        }

        // POST: Acts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("Title,Image,ImageHash")] ActInfo act)
        {
            var imageHash = await SaveImageContent(act);

            if (ModelState.IsValid)
            {
                act.ActGuid = id;
                act.ImageHash = imageHash;
                await actCommands.SaveAct(act);
                return RedirectToAction(nameof(Index));
            }
            return View(act);
        }

        // GET: Acts/Edit/abc-123
        public async Task<IActionResult> Edit(Guid id)
        {
            var actInfo = await actQueries.GetAct(id);
            if (actInfo == null)
            {
                return NotFound();
            }
            return View(actInfo);
        }

        // POST: Acts/Edit/abc-123
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Image,ImageHash,LastModifiedTicks")] ActInfo act)
        {
            var imageHash = await SaveImageContent(act);

            if (ModelState.IsValid)
            {
                act.ActGuid = id;
                act.ImageHash = imageHash;
                await actCommands.SaveAct(act);
                return RedirectToAction(nameof(Index));
            }
            return View(act);
        }

        // GET: Acts/Delete/abc-123
        public async Task<IActionResult> Delete(Guid id)
        {
            var actInfo = await actQueries.GetAct(id);
            if (actInfo == null)
            {
                return NotFound();
            }

            return View(actInfo);
        }

        // POST: Acts/Delete/abc-123
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await actCommands.RemoveAct(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageContent(ActInfo act)
        {
            if (act.Image != null)
            {
                using var imageReadStream = act.Image.OpenReadStream();
                using var imageMemoryStream = new MemoryStream();
                await imageReadStream.CopyToAsync(imageMemoryStream);
                var imageHash = await contentCommands.SaveContent(imageMemoryStream.ToArray(), act.Image.ContentType);
                return imageHash;
            }
            else
            {
                return act.ImageHash;
            }
        }
    }
}
