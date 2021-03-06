﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwentureLogs2016Data.Shared.Models.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using products_api_istest.AzureProviders;


namespace products_api_istest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {

        private readonly AzureRepository _azureRepo;
        private readonly IHostingEnvironment _hostingEnv;
        public DocumentsController(IHostingEnvironment hostingEnv, IConfiguration _config)
        {
            this._hostingEnv = hostingEnv;
            _azureRepo = new AzureRepository(_hostingEnv, _config);            
        }

        // GET: api/Documents/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Documents
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocumentDto document)
        {
            var etag  = await _azureRepo.SaveDocToAzure(document);
            return Ok(etag);
        }

        //// PUT: api/Documents/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
