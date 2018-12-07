using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwentureLogs2016Data.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace products_api_istest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AdventureWorks2016Context _context;
        // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProductsController));
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AdventureWorks2016Context context, ILogger<ProductsController> logger)
        {
            _logger = logger;
            _context = context;
            _logger.LogDebug(1, "NLog injected into Controller");
        }

    // GET: api/Products
    [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            _logger.LogDebug("Hello logging world!");
            try
            {
                return _context.Product.ToArray();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var nextId = _context.GetNextDocId();
                var product = await _context.Product.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        // GET: api/Products/name/testprod
        [HttpGet("name/{prodName}")]
        public IActionResult GetProductByName([FromRoute] string prodName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var products =  _context.Product.Where(x => x.Name.Contains(prodName)).Select(x =>x);

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.ToString());
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}