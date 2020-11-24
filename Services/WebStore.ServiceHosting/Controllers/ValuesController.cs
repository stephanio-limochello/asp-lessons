using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.ServiceHosting.Controllers
{
	[Route("api/v1/values")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private static readonly List<string> __values = Enumerable.Range(0, 10).Select(x => $"Value {x}").ToList();

		[HttpGet]
		public IEnumerable<string> Get()
		{
			return __values;
		}

		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			if (id < 0) return BadRequest();
			if (id >= __values.Count) return NotFound();
			return __values[id];
		}

		[HttpPost]
		public void Post([FromBody] string value) => __values.Add(value);

		[HttpPut("{id}")]
		public ActionResult Put(int id, [FromBody] string value)
		{
			if (id < 0) return BadRequest();
			if (id >= __values.Count) return NotFound();
			__values[id] = value;
			return Ok();
		}

		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			if (id < 0) return BadRequest();
			if (id >= __values.Count) return NotFound();
			__values.RemoveAt(id);
			return Ok();
		}
	}
}
