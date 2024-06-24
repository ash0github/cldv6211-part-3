using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft.Data;
using KhumaloCraft.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents;
using Azure;
using System.Diagnostics;

namespace KhumaloCraft.Controllers
{
    public class ProductInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductInformationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ProductInformations
        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Merchant"))
            {
                var user = _userManager.GetUserId(User);
                var items = await _context.ProductInformation.Where(i => i.UserID == user).ToListAsync();

                return View(items);
            }
            else
            {
                var items = await _context.ProductInformation.ToListAsync();
                return View(items);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Index(SearchData model)
        {
            try
            {
                // Check for a search string
                if (model.searchText == null)
                {
                    model.searchText = "";
                }

                await RunQueryAsync(model);
            }

            catch
            {
                return View("Error", new ErrorViewModel { RequestId = "1" });
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static SearchClient _searchClient;
        private static SearchIndexClient _indexClient;
        private static IConfigurationBuilder _builder;
        private static IConfigurationRoot _configuration;

        private void InitSearch()
        {
            // Create a configuration using appsettings.json
            _builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = _builder.Build();

            // Read the values from appsettings.json
            string searchServiceUri = _configuration["SearchServiceUri"];
            string queryApiKey = _configuration["SearchServiceQueryApiKey"];

            // Create a service and index client.
            _indexClient = new SearchIndexClient(new Uri(searchServiceUri), new AzureKeyCredential(queryApiKey));
            _searchClient = _indexClient.GetSearchClient("hotels-sample-index");
        }

        private async Task<ActionResult> RunQueryAsync(SearchData model)
        {
            InitSearch();

            var options = new SearchOptions()
            {
                IncludeTotalCount = true
            };

            // Enter 'property names' to specify which fields are returned.
            // If Select is empty, all "retrievable" fields are returned.
            options.Select.Add("ProductName");
            options.Select.Add("ProductPrice");
            options.Select.Add("ProductCategory");
            options.Select.Add("ProductAvailability");

            // For efficiency, the search call should be asynchronous, so use SearchAsync rather than Search.
            model.resultList = await _searchClient.SearchAsync<ProductInformation>(model.searchText, options).ConfigureAwait(false);

            return View("Index", model);
        }

        // GET: Pottery
        public async Task<IActionResult> ShowPottery()
        {
            return View("Index", await _context.ProductInformation.Where(i => i.ProductCategory.Contains("Pottery")).ToListAsync());
        }

        // GET: Crochet
        public async Task<IActionResult> ShowCrochet()
        {
            return View("Index", await _context.ProductInformation.Where(i => i.ProductCategory.Contains("Crochet")).ToListAsync());
        }

        // GET: Lapidary
        public async Task<IActionResult> ShowLapidary()
        {
            return View("Index", await _context.ProductInformation.Where(i => i.ProductCategory.Contains("Lapidary")).ToListAsync());
        }

        // GET: MetalWork
        public async Task<IActionResult> ShowMetalWork()
        {
            return View("Index", await _context.ProductInformation.Where(i => i.ProductCategory.Contains("MetalWork")).ToListAsync());
        }

        // GET: ProductInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInformation = await _context.ProductInformation
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (productInformation == null)
            {
                return NotFound();
            }

            return View(productInformation);
        }

        // GET: ProductInformations/Create
        [Authorize(Roles = "Merchant")]
        public IActionResult Create()
        {
            List<string> category = new List<string>()
            {
                "Pottery", "Crochet", "Lapidary", "MetalWork"
            };

            ViewBag.Category = new SelectList(category);

            return View();
        }

        // POST: ProductInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Merchant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductPrice,ProductCategory,ProductAvailability")] ProductInformation productInformation)
        {
                var user =  await _userManager.GetUserAsync(User);
                productInformation.UserID = user.Id;

                _context.Add(productInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: ProductInformations/Edit/5
        [Authorize(Roles = "Merchant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInformation = await _context.ProductInformation.FindAsync(id);
            if (productInformation == null)
            {
                return NotFound();
            }
            return View(productInformation);
        }

        // POST: ProductInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Merchant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductPrice,ProductCategory,ProductAvailability")] ProductInformation productInformation)
        {
            var user = await _userManager.GetUserAsync(User);
            productInformation.UserID = user.Id;

            if (id != productInformation.ProductID)
            {
                return NotFound();
            }

            try
            {
                _context.Update(productInformation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductInformationExists(productInformation.ProductID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductInformations/Delete/5
        [Authorize(Roles = "Merchant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInformation = await _context.ProductInformation
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (productInformation == null)
            {
                return NotFound();
            }

            return View(productInformation);
        }

        // POST: ProductInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInformation = await _context.ProductInformation.FindAsync(id);
            if (productInformation != null)
            {
                _context.ProductInformation.Remove(productInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInformationExists(int id)
        {
            return _context.ProductInformation.Any(e => e.ProductID == id);
        }
    }
}
