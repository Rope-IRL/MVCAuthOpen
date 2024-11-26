using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCAuth;
using MVCAuth.Controllers;
using MVCAuth.Controllers.ModelControllers;
using MVCAuth.ModelViews;
using MVCAuth.Services;
using Xunit.Abstractions;

namespace Tests;

public class HouseTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ServiceProvider _serviceProvider;

    public HouseTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _serviceProvider = BuildTestServiceProvider();
    }

    private ServiceProvider BuildTestServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        // Register the in-memory database
        serviceCollection.AddDbContext<RealEstateRentalContext>(options =>
            options.UseInMemoryDatabase("Name"));

        // Register IMemoryCache
        serviceCollection.AddMemoryCache();

        // Register services for testing
        serviceCollection.AddScoped<HouseService>();
        return serviceCollection.BuildServiceProvider();
    }
    
    [Fact]
    public async Task Put_UpdatesFlat_WhenFlatExists()
    {
        // Arrange
        using var serviceProvider = BuildTestServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateRentalContext>();

        // Seed the database with a flat
        var house = new House
        {
            Header = "Old Header",
            Description = "Old Description",
            AvgMark = 3.5m,
            City = "Old City",
            Address = "123 Old St",
            NumberOfRooms = 1,
            NumberOfFloors = 1,
            BathroomAvailability = false,
            WiFiAvailability = false,
            CostPerDay = 50.0m
        };
        dbContext.Houses.RemoveRange(dbContext.Houses);
        await dbContext.SaveChangesAsync();
        dbContext.Houses.Add(house);
        await dbContext.SaveChangesAsync();

        var houseService = scope.ServiceProvider.GetRequiredService<HouseService>();
        var controller = new HouseController(houseService);

        // Act: Update the flat
        house.Header = "Updated Header";
        house.Description = "Updated Description";
        var result = await controller.Put(house);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var updatedFlat = Assert.IsAssignableFrom<House>(actionResult.Value);
        Assert.Equal("Updated Header", updatedFlat.Header);
        Assert.Equal("Updated Description", updatedFlat.Description);
    }
    
    

    [Fact]
    public async Task Delete_ReturnsOk_WhenFlatIsDeleted()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateRentalContext>();

        // Seed the database with a flat
        var house = new House
        {
            Header = "Flat to Delete",
            Description = "Description of flat to delete",
            AvgMark = 3.5m,
            City = "City to Delete",
            Address = "123 Delete St",
            NumberOfRooms = 2,
            NumberOfFloors = 1,
            BathroomAvailability = true,
            WiFiAvailability = true,
            CostPerDay = 100.0m
        };
        dbContext.Houses.RemoveRange(dbContext.Houses);
        dbContext.SaveChanges();
        dbContext.Houses.Add(house);
        await dbContext.SaveChangesAsync();

        var flatService = scope.ServiceProvider.GetRequiredService<HouseService>();
        var controller = new HouseController(flatService);

        int numberOfFlatsBeforeDelete = dbContext.Flats.Count();
        

        // Act: Delete the flat
        var result = await controller.Delete(house.Pid);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var deletedFlatId = Assert.IsType<int>(okResult.Value);

        // Verify that the number of flats is now 0
        var numberOfFlatsAfterDelete = dbContext.Houses.Count();
        Assert.Equal(0, numberOfFlatsAfterDelete);
    }

    [Fact]
    public async Task Filter_ReturnsOk_WhenFlatIsDeleted()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateRentalContext>();

        // Seed the database with a flat
        var house1 = new House
        {
            Header = "Flat to Delete",
            Description = "Description of flat to delete",
            AvgMark = 3.5m,
            City = "City1",
            Address = "Address1",
            NumberOfRooms = 2,
            NumberOfFloors = 1,
            BathroomAvailability = true,
            WiFiAvailability = true,
            CostPerDay = 170.0m
        };
        
        var house2 = new House
        {
            Header = "Flat to Delete",
            Description = "Description of flat to delete",
            AvgMark = 3.4m,
            City = "City2",
            Address = "Address2",
            NumberOfRooms = 2,
            NumberOfFloors = 1,
            BathroomAvailability = true,
            WiFiAvailability = true,
            CostPerDay = 110.0m
        };
        
        var house3 = new House
        {
            Header = "Flat to Delete",
            Description = "Description of flat to delete",
            AvgMark = 3.3m,
            City = "City1",
            Address = "Address4",
            NumberOfRooms = 2,
            NumberOfFloors = 2,
            BathroomAvailability = true,
            WiFiAvailability = true,
            CostPerDay = 170.0m
        };
        dbContext.Houses.RemoveRange(dbContext.Houses);
        dbContext.SaveChanges();
        dbContext.Houses.Add(house1);
        dbContext.Houses.Add(house2);
        dbContext.Houses.Add(house3);
        await dbContext.SaveChangesAsync();

        var flatService = scope.ServiceProvider.GetRequiredService<HouseService>();
        var controller = new HouseController(flatService);

        int numberOfFlatsBeforeDelete = dbContext.Flats.Count();
        
        var result = await controller.HouseFilter("City1", 169);
        
        var res = Assert.IsType<ActionResult<List<House>>>(result);
        var viewResult = (ViewResult)res.Result;
        var models = viewResult.Model as List<House>;
        
        // Verify that the number of flats is now 0
        Assert.Equal(2, models.Count);
    }
}