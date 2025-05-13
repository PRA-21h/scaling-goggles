using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> _products = new List<Product>();
        private static int _nextId = 1;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;

            if (_products.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    _products.Add(new Product
                    {
                        Id = _nextId++,
                        Name = $"Product {_nextId}",
                        Description = "Example",
                        CreationDate = DateTime.Now.AddDays(-i),
                        OnStorage = i * 3
                    });
                }
            }
        }

        // ✅ READ ALL
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        // ✅ READ ONE
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // ✅ CREATE
        [HttpPost]
        public IActionResult Create(ProductCreateDto dto)
        {
            var product = new Product
            {
                Id = _nextId++,
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = dto.CreationDate,
                OnStorage = dto.OnStorage
            };

            _products.Add(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductCreateDto dto)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.OnStorage = dto.OnStorage;
            existing.CreationDate = dto.CreationDate;

            return NoContent();
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            _products.Remove(existing);
            return NoContent();
        }
    }
}
