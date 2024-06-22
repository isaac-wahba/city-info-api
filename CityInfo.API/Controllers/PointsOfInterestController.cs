﻿using CityInfo.API.Models;
using CityInfo.API.validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly IValidator<PointOfInterestSaveDto> _pointOfInterestSaveValidator;

        public PointsOfInterestController(PointOfInterestSaveValidator pointOfInterestSaveValidator)
        {
            _pointOfInterestSaveValidator = pointOfInterestSaveValidator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if(city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);

        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.
                Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);

        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestSaveDto pointOfInterestSaveDto)
        {

            // The commented logic is implemented implecitely in the ApiController attribute
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            ValidationResult results = _pointOfInterestSaveValidator.Validate(pointOfInterestSaveDto);

            if (!results.IsValid)
            {
                // map over errors and return validation messages only
                return BadRequest(results.Errors.Select(e => e.ErrorMessage));
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null) return NotFound();

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var pointOfInterestToSave = new PointOfInterestDto
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestSaveDto.Name,
                Description = pointOfInterestSaveDto.Description
            };

            city.PointsOfInterest.Add(pointOfInterestToSave); 

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId,
                pointOfInterestId = pointOfInterestToSave.Id
            }, pointOfInterestToSave);
             
        }
    }
}
