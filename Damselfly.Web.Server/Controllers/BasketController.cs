﻿using Damselfly.Core.DbModels;
using Damselfly.Core.DbModels.Models.APIModels;
using Damselfly.Core.Models;
using Damselfly.Core.ScopedServices;
using Damselfly.Core.ScopedServices.Interfaces;
using Damselfly.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Damselfly.Web.Server.Controllers;

// [Authorize]
[ApiController]
[Route("/api/basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _service;

    private readonly ILogger<BasketController> _logger;

    public BasketController(BasketService service, ILogger<BasketController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpDelete("/api/basket/{basketId}")]
    public async Task DeleteBasket( int basketId)
    {
        _service.Delete(basketId);
    }

    [HttpPost("/api/basket")]
    public async Task<Basket> SetBasketState(BasketCreateRequest req)
    {
        return await _service.Create(req.Name, req.UserId);
    }

    [HttpGet("/api/basket/{basketId}")]
    public async Task<Basket> GetBasketById(int basketId)
    {
        return await _service.GetBasketById(basketId);
    }

    [HttpGet("/api/baskets/{userId}")]
    public async Task<ICollection<Basket>> GetUserBaskets(int userId)
    {
        return await _service.GetUserBaskets(userId);
    }

    [HttpGet("/api/basketdefault/{userId}")]
    public async Task<Basket> GetDefaultUserBasket(int userId)
    {
        return await _service.GetDefaultBasket(userId);
    }

    [HttpGet("/api/baskets")]
    public async Task<ICollection<Basket>> GetUserBaskets()
    {
        return await _service.GetUserBaskets(null);
    }

    [HttpPost("/api/basketimage/state")]
    public async Task SetBasketState( BasketStateRequest req )
    {
        await _service.SetImageBasketState(req.BasketId, req.NewState, req.ImageIds );
    }
}

