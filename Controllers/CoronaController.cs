using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiseaseDataProvider.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace diseasedataprovider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoronaController : ControllerBase
    {
        private readonly IStateDataProvider _stateDataProvider;

        private readonly ILogger<CoronaController> _logger;

        public CoronaController(ILogger<CoronaController> logger, IStateDataProvider stateDataProvider)
        {
            _logger = logger;
            _stateDataProvider = stateDataProvider;
        }

        [HttpGet]
        [Route("GetTotalCasesByState")]
        public int GetTotalCasesByState(string state_name)
        {
            return _stateDataProvider.get_total_confirmed_cases_by_state(state_name);
        }
    }
}
