using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// Gets Current Count of Indian Nationals in India infected for a particular state
        /// </summary>
        /// <response code="400">Provided State Name is empty</response>
        /// <response code="404">State Name not found in the Database</response>
        [HttpGet]
        [Route("GetTotalIndianCasesByState")]
        public ActionResult<object> GetTotalIndianCasesByState(string state_name)
        {
            if (string.IsNullOrEmpty(state_name))
            {
                return BadRequest("Invalid State Name");
            }
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

        /// <summary>
        /// Gets Current Count of Foreign Nationals in India infected for a particular state
        /// </summary>
        /// <response code="400">Provided State Name is empty</response>
        /// <response code="404">State Name not found in the Database</response>
        [HttpGet]
        [Route("GetTotalNonIndianCasesByState")]
        public ActionResult<object> GetTotalNonIndianCasesByState(string state_name)
        {
            if (string.IsNullOrEmpty(state_name))
            {
                return BadRequest("Invalid State Name");
            }
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

        /// <summary>
        /// Gets Exact Names of all States in DataBase.
        /// </summary>
        /// <response code="404">No State Data found in DB</response>
        [HttpGet]
        [Route("GetAllStateNames")]
        public ActionResult<object> GetAllStateNames()
        {
            List<string> state_list = new List<string>();
            try
            {
                state_list = _stateDataProvider.get_all_state_names();
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (DataException)
            {
                return NotFound("State not found");
            }
            return Json(new { state_list = state_list });
        }

        /// <summary>
        /// Gets Total Cases in India on a particular date. Date Format should be mm/dd/yyyy
        /// </summary>
        /// <response code="404">No Data found for this date</response>
        /// <response code="400">Provided Date is invalid</response>
        [HttpGet]
        [Route("GetTotalCasesByDate")]
        public ActionResult<object> GetTotalCaseCountByDate(string date)
        {
            int count = 0;
            DateTime dt = default(DateTime);
            try
            {
                dt = string.IsNullOrWhiteSpace(date) ? DateTime.Now : Convert.ToDateTime(date);
                count = _stateDataProvider.get_total_case_count_by_date(dt);
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (FormatException)
            {
                return BadRequest("Date Provided is invalid");
            }
            catch (DataException)
            {
                return NotFound("No Data Found for the Date " + dt.Date.ToString("d"));
            }
            return Json(new { date = dt.Date.ToString("d"), total_cases_in_india = count });
        }

        /// <summary>
        /// Gets Total Cases in India on a particular date For a particular state. Date Format mm/dd/yyyy
        /// </summary>
        /// <response code="400">Provided State Name or Date is invalid</response>
        /// <response code="404">No Data found for the provided input</response>
        [HttpGet]
        [Route("GetTotalIndianCasesByDateForState")]
        public ActionResult<object> GetTotalCaseCountByDateForState(string date, string state)
        {
            int count = 0;
            DateTime dt = default(DateTime);
            try
            {
                if (string.IsNullOrWhiteSpace(state))
                {
                    return BadRequest("State cannot be empty");
                }
                dt = string.IsNullOrWhiteSpace(date) ? DateTime.Now : Convert.ToDateTime(date);
                count = _stateDataProvider.get_total_case_count_by_date(dt, state);
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (FormatException)
            {
                return BadRequest("Date Provided is invalid");
            }
            catch (DataException)
            {
                return NotFound("No Data Found for the Date " + dt.Date.ToString("d"));
            }
            return Json(new { date = dt.Date.ToString("d"), total_number_of_cases = count });
        }

        /// <summary>
        /// Gets Total Cases in India segregated by states
        /// </summary>
        [HttpGet]
        [Route("GetCurrentCaseCountForAllStates")]
        public ActionResult<object> GetCurrentCaseCountForAllStates()
        {
            var data = new object();
            try
            {
                data = _stateDataProvider.get_current_case_count_all_states();
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            return Json(data);
        }

        /// <summary>
        /// Gets cases count per day for everyday since inception of the Virus in a particular state
        /// </summary>
        /// <response code="400">Provided State Name is invalid</response>
        /// <response code="404">No Data found for the provided input</response>
        [HttpGet]
        [Route("GetHistoricalDataPerState")]
        public ActionResult<object> GetHistoricalDataPerState(string state_name)
        {
            var data = new object();
            try
            {
                if (string.IsNullOrWhiteSpace(state_name))
                {
                    return BadRequest("State cannot be empty");
                }
                data = _stateDataProvider.get_historical_data_per_state(state_name);
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (DataException)
            {
                return NotFound("State Name Not Found");
            }
            return Json(data);
        }

        /// <summary>
        /// Gets cumulative sum of cases per day for everyday since inception of the Virus in a particular state
        /// </summary>
        /// <response code="400">Provided State Name is invalid</response>
        /// <response code="404">No Data found for the provided input</response>
        [HttpGet]
        [Route("GetCumulativeHistoricalDataPerState")]
        public ActionResult<object> GetCumulativeHistoricalDataPerState(string state_name)
        {
            var data = new object();
            try
            {
                if (string.IsNullOrWhiteSpace(state_name))
                {
                    return BadRequest("State cannot be empty");
                }
                data = _stateDataProvider.get_cumulative_historical_data_per_state(state_name);
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (DataException)
            {
                return NotFound("State Name Not Found");
            }
            return Json(data);
        }

        /// <summary>
        /// Gets cases per day for A date range in a particular state
        /// </summary>
        /// <response code="400">Provided data is invalid</response>
        /// <response code="404">No Data found for the provided input</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("GetCasesForDateRangePerState")]
        public ActionResult<object> GetCasesForADateRangePerState(string state_name, string start_date, string end_date)
        {
            var data = new object();
            try
            {
                if (string.IsNullOrWhiteSpace(state_name) || string.IsNullOrWhiteSpace(start_date) || string.IsNullOrWhiteSpace(end_date))
                {
                    return BadRequest("All fields are mandatory");
                }
                var dt_start = Convert.ToDateTime(start_date);
                var dt_end = Convert.ToDateTime(end_date);

                if (dt_end < dt_start)
                {
                    throw new FormatException("Start Date cannot be after end date");
                }

                data = _stateDataProvider.get_cases_for_date_range_per_state(state_name, dt_start, dt_end);
            }
            catch (FormatException)
            {
                return BadRequest("Date Provided is invalid");
            }
            catch (SqlException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured while connecting to Database");
            }
            catch (DataException)
            {
                return NotFound("State Name Not Found");
            }
            return Json(data);
        }
    }
}
