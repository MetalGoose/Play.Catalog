using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItmesController : ControllerBase
    {
        //Temp data
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Power axe", "Deals a lot of damage", 120, DateTimeOffset.UtcNow)
        };

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        // GET /itmes/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);

            if (item is null) return NotFound();

            return item;
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var newItem = new ItemDto(
                    Guid.NewGuid(),
                    createItemDto.Name,
                    createItemDto.Description,
                    createItemDto.Price,
                    DateTimeOffset.UtcNow
                );

            items.Add(newItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        // PUT /itmes/{id}
        [HttpPut("{id}")]
        public ActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.FirstOrDefault(x => x.Id == id);

            if (existingItem is null) return NotFound();

            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(x => x.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(x => x.Id == id);

            if (index != -1)
            {
                items.RemoveAt(index);
                return NoContent();
            }

            return NotFound();
        }
    }
}