using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebStore.Interfaces.TestApi;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValueService _valueService;
		private readonly ILogger<WebAPIController> _logger;

		public WebAPIController(IValueService ValueService, ILogger<WebAPIController> logger) 
        { 
            _valueService = ValueService;
			_logger = logger;
		}

        public IActionResult Index()
        {
			try
            {
                var values = _valueService.Get();
                return View(values);
            }
			catch(Exception ex)
			{
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
        }


        public IActionResult GetById(int id)
        {
            try
            {
                var value = _valueService.Get(id);
                return Content(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string value)
        {
            try
            {
                if (!(value is null))
                {
                    //var url = _valueService.Post(value);
                    var url = await _valueService.PostAsync(value);
                    if (url == null) return RedirectToAction("index"); 
                    return Content(url.ToString());
                }
                else
                {
                    return BadRequest("Value is empty");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult Update() => View();

        [HttpPost]
        public IActionResult Update(int id, string value)
        {
            try
            {
                if (id > 0)
                {
                    var statusCode = _valueService.Update(id, value);
                    Response.StatusCode = (int)statusCode;
                    return Content($"{id} updated");
                }
                else
                {
                    return BadRequest("Id is unacceptable");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    var statusCode = _valueService.Delete(id);
                    Response.StatusCode = (int)statusCode;
                    return RedirectToAction("index");
                    //return Content($"{id} delete");
                }
                else
                {
                    return BadRequest("Id is unacceptable");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
        }
    }
}