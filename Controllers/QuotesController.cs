using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;
using QuotesApi.Validators;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {

        private QuotesDbContext _quotesDbContext;


        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        // GET: api/Quotes
        [HttpGet]
        public IActionResult Get(string sort)
        {
            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }
            return Ok(quotes);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchQuote(string type)
        {
            var quotes = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quotes);
        }

        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound(new { message = $"No record found with the id:{id}" });
            }

            return Ok(_quotesDbContext.Quotes.Find(id));
        }

        //[HttpGet("[action]/{id}")]
        //public int GetById(int id)
        //{
        //    return id;
        //}

        // POST: api/Quotes
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {

            var messages = QuoteValidation(quote);

            if (messages.Length > 0)
            {
                return NotFound(new { message = messages });
            }

            //if (results.IsValid) {}
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();
            return Created("", new { message = "Quote successfuly inserted" });
        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote newQuote)
        {
            var oldQuote = _quotesDbContext.Quotes.Find(id);

            var messages = QuoteValidation(oldQuote);

            if (messages.Length > 0)
            {
                return NotFound(new { message = messages });
            }

            oldQuote.Title = newQuote.Title;
            oldQuote.Author = newQuote.Author;
            oldQuote.Description = newQuote.Description;
            oldQuote.Type = newQuote.Type;
            oldQuote.CreatedAt = newQuote.CreatedAt;
            //oldQuote.CreatedAt = DateTime.ParseExact(newQuote.CreatedAt.ToShortDateString(), "yyyy-MM-dd",
            //    System.Globalization.CultureInfo.InvariantCulture);
            _quotesDbContext.SaveChanges();
            return Ok(new { message = "Quote updated successfuly" });
        } 

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound(new { message = $"No record found with the id:{id}" });
            }

            _quotesDbContext.Quotes.Remove(quote);
            _quotesDbContext.SaveChanges();
            return Ok(new { message = "Quote successfuly deleted" });
        }

        private static string[] QuoteValidation(Quote quote)
        {
            var validator = new QuoteValidator();
            ValidationResult results = validator.Validate(quote);
            var messages = new List<string>();
            if (results.Errors.Count > 0)
            {
                foreach (var error in results.Errors)
                {
                    messages.Add(error.ErrorMessage);
                }
            }
            return messages.ToArray();
        }

        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }
    }
}
