using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MVCAuth;
using MVCAuth.Controllers.ModelControllers;
using MVCAuth.ModelViews;
using MVCAuth.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class FlatControllerIntegrationTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ServiceProvider _serviceProvider;

    public FlatControllerIntegrationTests(ITestOutputHelper testOutputHelper)
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
        serviceCollection.AddScoped<FlatService>();
        serviceCollection.AddScoped<LandLordsAdditionalInfoService>();
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
        var flat = new Flat
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
        dbContext.Flats.RemoveRange(dbContext.Flats);
        await dbContext.SaveChangesAsync();
        dbContext.Flats.Add(flat);
        await dbContext.SaveChangesAsync();

        var flatService = scope.ServiceProvider.GetRequiredService<FlatService>();
        var landlordService = scope.ServiceProvider.GetRequiredService<LandLordsAdditionalInfoService>();
        var controller = new FlatController(flatService, landlordService);

        // Act: Update the flat
        flat.Header = "Updated Header";
        flat.Description = "Updated Description";
        var result = await controller.Put(flat);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var updatedFlat = Assert.IsAssignableFrom<Flat>(actionResult.Value);
        Assert.Equal("Updated Header", updatedFlat.Header);
        Assert.Equal("Updated Description", updatedFlat.Description);
    }
    

    [Fact]
    public async Task LandLordIndex_ReturnsViewWithFlatsAndLandlords()
    {
        // Arrange: Build the test service provider
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateRentalContext>();

        // Seed the database
        dbContext.Flats.Add(new Flat
        {
            Header = "Test Flat",
            Description = "Test Description",
            AvgMark = 4.5m,
            City = "Test City",
            Address = "123 Test St",
            NumberOfRooms = 2,
            NumberOfFloors = 1,
            BathroomAvailability = true,
            WiFiAvailability = true,
            CostPerDay = 100
        });

        dbContext.LandLordsAdditionalInfos.Add(new LandLordsAdditionalInfo
        {
            Llid = 1,
            Name = "Test Info",
            Surname = "sdfkjalsk",
            PassportId = "12312ID",
            Telephone = "123123"
        });

        await dbContext.SaveChangesAsync();

        var flatService = scope.ServiceProvider.GetRequiredService<FlatService>();
        var landlordService = scope.ServiceProvider.GetRequiredService<LandLordsAdditionalInfoService>();
        var controller = new FlatController(flatService, landlordService);

        // Act
        var result = await controller.LandLordIndex();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FlatModelView>(viewResult.Model);

        Assert.NotNull(model.Flats);
        Assert.Single(model.LandlordsInfo);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenFlatIsDeleted()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateRentalContext>();

        // Seed the database with a flat
        var flat = new Flat
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
        dbContext.Flats.RemoveRange(dbContext.Flats);
        dbContext.SaveChanges();
        dbContext.Flats.Add(flat);
        await dbContext.SaveChangesAsync();

        var flatService = scope.ServiceProvider.GetRequiredService<FlatService>();
        var landlordService = scope.ServiceProvider.GetRequiredService<LandLordsAdditionalInfoService>();
        var controller = new FlatController(flatService, landlordService);

        int numberOfFlatsBeforeDelete = dbContext.Flats.Count();
        

        // Act: Delete the flat
        var result = await controller.Delete(flat.Fid ?? 0);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var deletedFlatId = Assert.IsType<int>(okResult.Value);

        // Verify that the number of flats is now 0
        var numberOfFlatsAfterDelete = dbContext.Flats.Count();
        Assert.Equal(0, numberOfFlatsAfterDelete);
    }

    [Fact]
    public async Task Filter_ReturnsOk_WhenFlatIsDeleted()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RealEstateRentalContext>();

        // Seed the database with a flat
        var flat1 = new Flat
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
        
        var flat2 = new Flat
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
        
        var flat3 = new Flat
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
        dbContext.Flats.RemoveRange(dbContext.Flats);
        dbContext.SaveChanges();
        dbContext.Flats.Add(flat1);
        dbContext.Flats.Add(flat2);
        dbContext.Flats.Add(flat3);
        await dbContext.SaveChangesAsync();

        var flatService = scope.ServiceProvider.GetRequiredService<FlatService>();
        var landlordService = scope.ServiceProvider.GetRequiredService<LandLordsAdditionalInfoService>();
        var controller = new FlatController(flatService, landlordService);

        int numberOfFlatsBeforeDelete = dbContext.Flats.Count();
        
        var result = await controller.FlatFilter("City1", 169);
        
        var res = Assert.IsType<ActionResult<List<Flat>>>(result);
        var viewResult = (ViewResult)res.Result;
        var models = viewResult.Model as List<Flat>;
        
        // Verify that the number of flats is now 0
        Assert.Equal(2, models.Count);
    }

}
