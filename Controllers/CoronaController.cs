using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DiseaseDataProvider.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace diseasedataprovider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoronaController : Controller
    {
        private readonly IStateDataProvider _stateDataProvider;

        private readonly ILogger<CoronaController> _logger;

        public CoronaController(ILogger<CoronaController> logger, IStateDataProvider stateDataProvider)
        {
            _logger = logger;
            _stateDataProvider = stateDataProvider;
        }

        [HttpGet]
        [Route("GetTotalIndianCasesByState")]
        public ActionResult<object> GetTotalIndianCasesByState(string state_name)
        {
            var count = 0;
            try
            {
                count = _stateDataProvider.get_total_confirmed_cases_ind_by_state(state_name);
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (ArgumentException)
            {
                return NotFound("State not found");
            }
            return Json(new { state_name = state_name, count_indian = count });
        }
        [HttpGet]
        [Route("GetTotalNonIndianCasesByState")]
        public ActionResult<object> GetTotalNonIndianCasesByState(string state_name)
        {
            var count = 0;
            try
            {
                count = _stateDataProvider.get_total_confirmed_cases_int_by_state(state_name);
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (ArgumentException)
            {
                return NotFound("State not found");
            }
            return Json(new { state_name = state_name, count_non_indian = count });
        }
    }
}
