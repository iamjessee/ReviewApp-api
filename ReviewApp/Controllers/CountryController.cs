﻿using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers
{
    // define route and API controller attributes
    [Route("api/[controller]")]
    [ApiController]

    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryrepository; // repo for country data access
        private readonly IMapper _mapper; // automapper for DTO conversion

        // constructor for injecting dependencies
        public CountryController(ICountryRepository countryrepository, IMapper mapper)
        {
            _countryrepository = countryrepository;
            _mapper = mapper;
        }

        // get all countries
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryrepository.GetCountries()); // map to DTO

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // return bad request if model state is invalid
            }

            return Ok(countries); // return countries
        }

        // get country by id
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryrepository.CountryExists(countryId))
            {
                return NotFound(); // return not found if country does not exist
            }

            var country = _mapper.Map<CountryDto>(_countryrepository.GetCountry(countryId)); // map to DTO

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // return bad request if model state is invalid
            }

            return Ok(country); // return country
        }

        // get country by owner id
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryrepository.GetCountryByOwner(ownerId)); // map to DTO

            if (!ModelState.IsValid)
            {
                return BadRequest(); // return bad request if model state is invalid
            }

            return Ok(country); // return country
        }
    }
}