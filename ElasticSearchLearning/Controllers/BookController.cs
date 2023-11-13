using ElasticSearchLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchLearning.wwwroot
{
    public class BookController : Controller
    {
        private readonly IElasticClient _elasticClient;
        public BookController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;    
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new SearchViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Index([FromForm]SearchViewModel searchModel)
        {
            ISearchResponse<Book> results;
            if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
            {
                results = await _elasticClient.SearchAsync<Book>(
                             s => s.Query(
                                 q => q.MatchPhrase(
                                     t => t.Field(a=>a.Title).Query(searchModel.Keyword))
                                 ));
            }
            else
            {
                results =  await _elasticClient.SearchAsync<Book>(s => s
                    .Query(q => q
                        .MatchAll()
                    )
                );
            }

            return View(new SearchViewModel { Books = results.Documents.ToList() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            var response = await _elasticClient.IndexDocumentAsync(book);

            if (response.Result == Result.Error) 
                return BadRequest();

            return Ok();
        }
    }
}
